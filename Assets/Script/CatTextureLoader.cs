using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CatTextureLoader : MonoBehaviour
{
    // 猫の画像を取得するためのAPI URL
    private string _urlAPI = "https://cataas.com/cat";

    // GachaData ScriptableObject の参照
    [SerializeField] GachaData _gachaData;

    // GachaSetting の参照 (排出率設定)
    [SerializeField] private GachaSetting _gachaSetting;

    [Tooltip("ガチャの天井")]
    [SerializeField] int _ceilingCount = 200;

    [SerializeField] Text _loadingText; // ローディングテキストの追加

    [SerializeField] private Image _singleButton;

    [SerializeField] private Image _tenButton;

    [SerializeField] private Image _changeButton;

    [SerializeField] private Image _probabilityButton;

    [SerializeField] private Text _remainingText; // 天井まで残り何回か

    [SerializeField] private Text _text; // UR確定までと表示

    // レア度ごとのシーン名
    [SerializeField] private string _normalScene = "";

    [SerializeField] private string _oneSSRScene = "";

    [SerializeField] private string _moreSSRScene = "";

    [SerializeField] private string _oneURScene = "";

    private bool _isGachaInProgress = false;

    private void Start()
    {
        // ゲーム開始時にデータを読み込む
        _gachaData.LoadData();

        // 初期化時に残り回数の表示を更新
        UpdateCatRemainingCount();
    }

    // 単発ガチャがクリックされたときに呼び出される
    public void OnSingleGachaClick()
    {
        if (_isGachaInProgress) return;

        _remainingText.gameObject.SetActive(false);
        _text.gameObject.SetActive(false);
        _isGachaInProgress = true;
        StartCoroutine(GetAPI(1));

        _singleButton.raycastTarget = false;
        _tenButton.raycastTarget = false;
        _changeButton.raycastTarget = false;
        _probabilityButton.raycastTarget = false;
    }

    // 10連ガチャがクリックされたときに呼び出される
    public void OnTenGachaClick()
    {
        if (_isGachaInProgress) return;

        _remainingText.gameObject.SetActive(false);
        _text.gameObject.SetActive(false);
        _isGachaInProgress = true;
        StartCoroutine(GetAPI(10));

        _singleButton.raycastTarget = false;
        _tenButton.raycastTarget = false;
        _changeButton.raycastTarget = false;
        _probabilityButton.raycastTarget = false;
    }

    // 指定された回数（count）の猫画像を取得し、GachaData に保存する
    IEnumerator GetAPI(int count)
    {
        _loadingText.gameObject.SetActive(true); // ローディングテキストを表示

        // GachaData 内の結果配列を初期化
        _gachaData.gachaResults = new GachaData.GachaResult[count];

        for (int i = 0; i < count; i++)
        {
            // レア度をランダムに決定
            Rarity selectedRarity = GetRandomRarity();

            // 天井システムの実装
            if (_gachaData.totalGachaCount % _ceilingCount == _ceilingCount - 1)
            {
                // 200回ごとに必ずURを出す
                selectedRarity = Rarity.UR;
            }
            else
            {
                selectedRarity = GetRandomRarity();
            }

            

#if UNITY_EDITOR
            //Debug.Log($"排出されたレア度: {selectedRarity}");
#endif

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(_urlAPI);

            // リクエストの送信と待機
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // ガチャ回数をインクリメント
                _gachaData.totalGachaCount++;

                _gachaData.SaveData(); // データを保存

                // ...成功時の処理...
                if (i == count - 1) // 最後の画像のロードが完了した場合
                {
                    _loadingText.gameObject.SetActive(false); // ローディングテキストを非表示に
                    _isGachaInProgress = false; // ガチャの進行状態をリセット
                    _remainingText.gameObject.SetActive(true);
                    _text.gameObject.SetActive(true);

                    _singleButton.raycastTarget = true;
                    _tenButton.raycastTarget = true;
                    _changeButton.raycastTarget = true;
                    _probabilityButton.raycastTarget = true;
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(request);

                // Editor 内でのみ画像のサイズをログ出力
#if UNITY_EDITOR
                Debug.Log($"Image Width: {texture.width}");
                Debug.Log($"Image Height: {texture.height}");
#endif

                // 取得したテクスチャとレア度を GachaData に保存
                _gachaData.gachaResults[i] = new GachaData.GachaResult
                {
                    texture = texture,
                    rarity = selectedRarity
                };
            }
            else
            {
                Debug.LogError($"リクエスト失敗: {request.error}");

                Animator animator = _loadingText.GetComponent<Animator>(); // テキストのアニメーターを取得
                if (animator != null)
                {
                    animator.enabled = false;  // アニメーションを一時的に無効化
                }
                _loadingText.color = Color.red;  // テキストの色を赤に変更
                _loadingText.text = "ロード失敗";  // エラーメッセージを設定
                yield return new WaitForSeconds(1.5f);

                // タイトルに戻る
                SceneManager.LoadScene("Title");
            }
        }

        _loadingText.gameObject.SetActive(false);

        // ガチャ結果の取得が完了
        Debug.Log("ガチャ結果の取得が完了しました");

        // レア度のカウントを取得
        int ssrCount = _gachaData.gachaResults.Count(result => result.rarity == Rarity.SSR);
        int urCount = _gachaData.gachaResults.Count(result => result.rarity == Rarity.UR);

        // 遷移先のシーンを決定
        string sceneToLoad = RarityChangeScene(ssrCount, urCount);
        SceneManager.LoadScene(sceneToLoad);
    }

    /// <summary>
    /// レア度に応じたシーン名を決定する
    /// </summary>
    /// <param name="ssrCount">SSRの枚数</param>
    /// <param name="urCount">URの枚数</param>
    /// <returns>シーン名</returns>
    private string RarityChangeScene(int ssrCount, int urCount)
    {
        if (urCount > 0)
        {
            return _oneURScene; // URが一枚以上の場合
        }
        else if (ssrCount >= 2)
        {
            return _moreSSRScene; // SSRが二枚以上の場合
        }
        else if (ssrCount == 1)
        {
            return _oneSSRScene; // SSRが一枚の場合
        }
        else
        {
            return _normalScene; // SSRもURもない場合
        }
    }

    /// <summary>
    /// レア度をランダムに決定する
    /// </summary>
    /// <returns></returns>
    private Rarity GetRandomRarity()
    {
        float total = 0f;
        foreach (var rate in _gachaSetting.rarityRates)
        {
            total += rate.rate;
        }

        float randomValue = Random.Range(0, total);
        float cumulative = 0f;

        foreach (var rate in _gachaSetting.rarityRates)
        {
            cumulative += rate.rate;
            if (randomValue <= cumulative)
            {
                return rate.rarity;
            }
        }

        return Rarity.R; // デフォルトは R
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateCatRemainingCount()
    {
        int remainingToUR = _ceilingCount - (_gachaData.totalGachaCount % _ceilingCount);
        _remainingText.text = $"残り {remainingToUR}回";
    }
}

