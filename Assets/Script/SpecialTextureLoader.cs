using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SpecialTextureLoader : MonoBehaviour
{
    // JSON から受信するデータ
    [Serializable]
    public class ResponseData
    {
        public string url;
    }

    [SerializeField] private GachaData _gachaData;
    [SerializeField] private GachaSetting _gachaSetting; // レア度設定
    private int _maxImages = 1;

    // 単発ガチャがクリックされたときに呼び出される
    public void OnSingleGachaClick()
    {
        _maxImages = 1; // 単発ガチャ
        StartCoroutine(GetAPI(_maxImages));
    }

    // 10連ガチャがクリックされたときに呼び出される
    public void OnTenGachaClick()
    {
        _maxImages = 10; // 10連ガチャ
        StartCoroutine(GetAPI(_maxImages));
    }

    // APIを使って画像を取得し、レア度と一緒にGachaDataに保存
    IEnumerator GetAPI(int count)
    {
        // ガチャ結果の配列を初期化
        _gachaData.gachaResults = new GachaData.GachaResult[count];

        for (int i = 0; i < count; i++)
        {
            // レア度をランダムに決定
            Rarity selectedRarity = GetRandomRarity();
            Debug.Log($"排出されたレア度: {selectedRarity}");

            UnityWebRequest request = UnityWebRequest.Get("https://nekos.best/api/v2/waifu");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                ResponseData response = JsonUtility.FromJson<ResponseData>(jsonResponse);
                yield return StartCoroutine(GetTexture(response.url, i, selectedRarity));
            }
            else
            {
                Debug.LogError($"画像取得失敗: {request.error}");
            }
        }

        // ガチャ結果の取得が完了
        Debug.Log("ガチャ結果の取得が完了しました");

        // ガチャが終わったらシーン遷移
        SceneManager.LoadScene("Gacha Main Scene"); // 画像表示シーンに遷移
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
            _gachaData.gachaResults[index] = new GachaData.GachaResult
            {
                texture = texture,
                rarity = rarity
            };

#if UNITY_EDITOR
            Debug.Log($"Image Width: {texture.width}");
            Debug.Log($"Image Height: {texture.height}");
#endif
        }
        else
        {
            Debug.LogError($"テクスチャ取得失敗: {request.error}");
        }
    }

    // レア度をランダムに決定
    private Rarity GetRandomRarity()
    {
        float total = 0f;
        foreach (var rate in _gachaSetting.rarityRates)
        {
            total += rate.rate;
        }

        float randomValue = UnityEngine.Random.Range(0, total);
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
