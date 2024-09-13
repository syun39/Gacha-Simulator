using System.Collections;
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

    [SerializeField] Text  _loadingText; // ローディングテキストの追加

    // ガチャの回数
    private int _maxImages = 1;

    private void Start()
    {
        _loadingText.gameObject.SetActive(false);
    }

    // 単発ガチャがクリックされたときに呼び出される
    public void OnSingleGachaClick()
    {
        _maxImages = 1; // 単発ガチャは1回だけ
        StartCoroutine(GetAPI(_maxImages));
        _loadingText.gameObject.SetActive(true);
    }

    // 10連ガチャがクリックされたときに呼び出される
    public void OnTenGachaClick()
    {
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
            Debug.Log($"排出されたレア度: {selectedRarity}");
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

        // ガチャが終わったらシーン遷移
        SceneManager.LoadScene("Normal direction Scene"); // 画像表示シーンに遷移
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
}

