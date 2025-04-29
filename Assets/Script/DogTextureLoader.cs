using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DogTextureLoader : MonoBehaviour
{
    // JSON から受信するデータ
    [Serializable]
    public class ResponseData
    {
        public string message; // 画像URL
    }

    [SerializeField] GachaData _gachaData; // GachaData

    [SerializeField] GachaSetting _gachaSetting; // 排出率

    [Tooltip("ガチャの天井")]
    [SerializeField] int _ceilingCount = 200; // 天井

    [SerializeField] Text _loadingText; // ローディングテキスト

    [SerializeField] Image _singleButton; // 単発ガチャボタン

    [SerializeField] Image _tenButton; // 10連ガチャボタン

    [SerializeField] Image _changeButton; // ガチャ切り替えボタン

    [SerializeField] Image _probabilityButton; // 確率表示ボタン

    [SerializeField] Text _remainingText; // 天井まで残り何回か

    [SerializeField] Text _text; // UR確定までと表示

    // レア度ごとのシーン名
    [SerializeField] string _normalScene = ""; // 爆死シーン

    [SerializeField] string _oneSSRScene = ""; // SSR確定シーン

    [SerializeField] string _moreSSRScene = ""; // SSR2枚以上シーン

    [SerializeField] string _oneURScene = ""; // UR確定シーン

    private bool _isGachaInProgress = false; // ガチャが進行中か

    private void Start()
    {
        // ゲーム開始時にデータを読み込む
        _gachaData.LoadData();

        // 残り回数の表示を更新
        UpdateDogRemainingCount();
    }

    /// <summary>
    /// 単発ガチャ
    /// </summary>
    public void OnSingleGachaClick()
    {
        // ガチャが進行中の場合
        if (_isGachaInProgress) return;

        _remainingText.gameObject.SetActive(false); // 天井まで残り何回かを非表示
        _text.gameObject.SetActive(false); //UR確定までを非表示
        StartCoroutine(GetAPI(1)); // 1回実行
        _isGachaInProgress = true; // ガチャが進行中

        // ボタンを無効化する処理
        _singleButton.raycastTarget = false;
        _tenButton.raycastTarget = false;
        _changeButton.raycastTarget = false;
        _probabilityButton.raycastTarget = false;
    }

    /// <summary>
    /// 10連ガチャ
    /// </summary>
    public void OnTenGachaClick()
    {
        // ガチャが進行中の場合
        if (_isGachaInProgress) return;

        _remainingText.gameObject.SetActive(false); // 天井まで残り何回かを非表示
        _text.gameObject.SetActive(false); //UR確定までを非表示
        StartCoroutine(GetAPI(10)); // 10回実行
        _isGachaInProgress = true; // ガチャが進行中

        // ボタンを無効化する処理
        _singleButton.raycastTarget = false;
        _tenButton.raycastTarget = false;
        _changeButton.raycastTarget = false;
        _probabilityButton.raycastTarget = false;
    }

    // APIを使って画像を取得し、レア度と一緒にGachaDataに保存
    IEnumerator GetAPI(int count)
    {
        _loadingText.gameObject.SetActive(true); // ローディングテキストを表示

        // GachaData 内の結果配列を初期化
        _gachaData.GachaResults = new GachaData.GachaResult[count];

        for (int i = 0; i < count; i++)
        {
            Rarity selectedRarity; // レア度選択

            // レア度をランダムに決定
            if (_gachaData.TotalGachaCount % _ceilingCount == _ceilingCount - 1) // 余りが一致したとき
            {
                // 天井の場合は必ずURを出す
                selectedRarity = Rarity.UR;
            }
            else if (count == 10 && i == count - 1) // 10連ガチャの最後はSR以上
            {
                float randomValue = UnityEngine.Random.Range(0f, 100f);
                if (randomValue < 1) // 1%でUR
                {
                    selectedRarity = Rarity.UR;
                }
                else if (randomValue < 4) // 3%でSSR
                {
                    selectedRarity = Rarity.SSR;
                }
                else // 残りの96%でSR
                {
                    selectedRarity = Rarity.SR;
                }
            }
            else
            {
                // 通常のガチャでレア度をランダムに決定
                selectedRarity = GetRandomRarity();
            }

            // APIリクエストを送信
            UnityWebRequest request = UnityWebRequest.Get("https://dog.ceo/api/breeds/image/random");
            yield return request.SendWebRequest();

            // 成功したら
            if (request.result == UnityWebRequest.Result.Success)
            {
                // レスポンス(応答)の処理
                string jsonResponse = request.downloadHandler.text;
                ResponseData response = JsonUtility.FromJson<ResponseData>(jsonResponse);
                yield return StartCoroutine(GetTexture(response.message, i, selectedRarity));

                if (i == count - 1) // 最後の画像のロードが完了した場合
                {
                    _loadingText.gameObject.SetActive(false); // ローディングテキストを非表示に
                    _isGachaInProgress = false; // ガチャの進行状態をリセット
                    _remainingText.gameObject.SetActive(true);
                    _text.gameObject.SetActive(true);

                    // ボタンを再度有効にする処理
                    _singleButton.raycastTarget = true;
                    _tenButton.raycastTarget = true;
                    _changeButton.raycastTarget = true;
                    _probabilityButton.raycastTarget = true;
                }
            }
            else
            {
                //Debug.LogError($"画像取得失敗: {request.error}");

                Animator animator = _loadingText.GetComponent<Animator>(); // テキストのアニメーターを取得

                if (animator != null)
                {
                    animator.enabled = false;  // アニメーションを一時的に無効化
                }
                _loadingText.color = Color.red;
                _loadingText.text = "ロード失敗"; // エラーメッセージに更新

                yield return new WaitForSeconds(1.5f);

                // タイトルに戻る
                SceneManager.LoadScene("Title");
            }
        }

        // ガチャ結果の取得が完了
        //Debug.Log("ガチャ結果の取得が完了しました");

        // ガチャ回数をインクリメント
        _gachaData.TotalGachaCount += count;

        _gachaData.SaveData(); // データを保存

        // レア度のカウント
        int ssrCount = 0;
        int urCount = 0;

        // ガチャ結果をループしてレア度をカウント
        foreach (var result in _gachaData.GachaResults)
        {
            if (result.rarity == Rarity.SSR)
            {
                ssrCount++; // SSRの場合カウントを増やす
            }
            else if (result.rarity == Rarity.UR)
            {
                urCount++; // URの場合カウントを増やす
            }
        }

        // 遷移先のシーンを決定
        string sceneToLoad = RarityChangeScene(ssrCount, urCount);
        SceneManager.LoadScene(sceneToLoad);
    }

    // テクスチャを取得して GachaData に保存
    IEnumerator GetTexture(string url, int index, Rarity rarity)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);

            // GachaData に画像とレア度を保存
            _gachaData.GachaResults[index] = new GachaData.GachaResult
            {
                texture = texture,
                rarity = rarity
            };

            //Debug.Log($"Image Width: {texture.width}");
            //Debug.Log($"Image Height: {texture.height}");

        }
        else
        {
            //Debug.LogError($"テクスチャ取得失敗: {request.error}");

            Animator animator = _loadingText.GetComponent<Animator>(); // テキストのアニメーターを取得

            if (animator != null)
            {
                animator.enabled = false;  // アニメーションを一時的に無効化
            }
            _loadingText.color = Color.red;
            _loadingText.text = "テクスチャ取得失敗"; // エラーメッセージに更新

            yield return new WaitForSeconds(1.5f);

            // タイトルに戻る
            SceneManager.LoadScene("Title");
        }

    }

    /// <summary>
    /// レア度に応じたシーン名を決定する
    /// </summary>
    /// <param name="ssrCount">SSRの枚数</param>
    /// <param name="urCount">URの枚数</param>
    /// <returns>シーン名</returns>
    string RarityChangeScene(int ssrCount, int urCount)
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
    /// レア度をランダムに決定(重み付きの確率抽選)
    /// </summary>
    Rarity GetRandomRarity()
    {
        float total = 0f;
        foreach (var rate in _gachaSetting.RarityRates)
        {
            total += rate.rate; // 各レア度の確率を合計する
        }

        float randomValue = UnityEngine.Random.Range(0, total); // ランダムな値を生成
        float cumulative = 0f; // 確率の累積値を保持

        foreach (var rate in _gachaSetting.RarityRates)
        {
            cumulative += rate.rate; // 確率の累積値を計算
            // ランダムな値が累積確率以下なら
            if (randomValue <= cumulative)
            {
                return rate.rarity; // ランダムな値に対応するレア度を返す
            }
        }
        // Rは0から70
        // SRは70.1から96
        // SSRは96.1から99
        // URは99.1から100
        return Rarity.R; // デフォルトは R
    }

    /// <summary>
    /// 天井まで残りのガチャ回数を更新
    /// </summary>
    void UpdateDogRemainingCount()
    {
        // 現在のガチャ回数を求める _ceilingCount - 余り
        int remainingToUR = _ceilingCount - (_gachaData.TotalGachaCount % _ceilingCount);
        _remainingText.text = $"残り {remainingToUR}回";
    }
}
