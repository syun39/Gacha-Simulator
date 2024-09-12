using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CatTextureLoader : MonoBehaviour
{
    // 猫の画像を取得するためのAPI URL
    private string _urlAPI = "https://cataas.com/cat?width=800&height=500";

    // GachaData ScriptableObject の参照
    [SerializeField] GachaData _gachaData;

    // GachaSetting の参照 (排出率設定)
    [SerializeField] private GachaSetting _gachaSetting;

    // ガチャの回数
    private int _maxImages = 1;

    // 単発ガチャがクリックされたときに呼び出される
    public void OnSingleGachaClick()
    {
        _maxImages = 1; // 単発ガチャは1回だけ
        StartCoroutine(GetAPI(_maxImages));
    }

    // 10連ガチャがクリックされたときに呼び出される
    public void OnTenGachaClick()
    {
        _maxImages = 10; // 10連ガチャは10回
        StartCoroutine(GetAPI(_maxImages));
    }

    // 指定された回数（count）の猫画像を取得し、GachaData に保存する
    IEnumerator GetAPI(int count)
    {
        // GachaData 内の結果配列を初期化
        _gachaData.gachaResults = new GachaData.GachaResult[count];

        for (int i = 0; i < count; i++)
        {
            // レア度をランダムに決定
            Rarity selectedRarity = GetRandomRarity();
            Debug.Log($"排出されたレア度: {selectedRarity}");

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(_urlAPI);

            // リクエストの送信と待機
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
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
            }
        }

        // ガチャ結果の取得が完了
        Debug.Log("ガチャ結果の取得が完了しました");

        // ガチャが終わったらシーン遷移
        SceneManager.LoadScene("Cat Dog Gacha Main Scene"); // 画像表示シーンに遷移
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

