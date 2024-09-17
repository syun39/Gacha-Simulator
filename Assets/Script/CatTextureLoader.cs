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

    [SerializeField] Text _loadingText; // ローディングテキストの追加

    // レア度ごとのシーン名
    [SerializeField] private string _normalScene = "";

    [SerializeField] private string _oneSSRScene = "";

    [SerializeField] private string _moreSSRScene = "";

    [SerializeField] private string _oneURScene = "";

    // ガチャの回数
    private int _maxImages = 1;

    private bool _isGachaInProgress = false;

    // 単発ガチャがクリックされたときに呼び出される
    public void OnSingleGachaClick()
    {
        if (_isGachaInProgress) return;

        ResetGacha(); // ガチャの状態をリセット
        _isGachaInProgress = true;
        _maxImages = 1; // 単発ガチャは1回だけ
        StartCoroutine(GetAPI(_maxImages));
        _loadingText.gameObject.SetActive(true);
    }

    // 10連ガチャがクリックされたときに呼び出される
    public void OnTenGachaClick()
    {
        if (_isGachaInProgress) return;

        ResetGacha(); // ガチャの状態をリセット
        _isGachaInProgress = true;
        _maxImages = 10; // 10連ガチャは10回
        StartCoroutine(GetAPI(_maxImages));
        _loadingText.gameObject.SetActive(true);
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

#if UNITY_EDITOR
            //Debug.Log($"排出されたレア度: {selectedRarity}");
#endif

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(_urlAPI);

            // リクエストの送信と待機
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // ...成功時の処理...
                if (i == count - 1) // 最後の画像のロードが完了した場合
                {
                    _loadingText.gameObject.SetActive(false); // ローディングテキストを非表示に
                    _isGachaInProgress = false; // ガチャの進行状態をリセット
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

                _loadingText.text = "ロード失敗"; // エラーメッセージに更新
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

    public void ResetGacha()
    {
        _gachaData.gachaResults = new GachaData.GachaResult[0]; // 空の配列に設定
        //_loadingText.gameObject.SetActive(false); // ローディングテキストを非表示
        _isGachaInProgress = false; // ガチャの進行状態をリセット
    }
}

