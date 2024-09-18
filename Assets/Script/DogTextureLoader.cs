using System;
using System.Collections;
using System.Linq;
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
        public string message;
    }

    [SerializeField] private GachaData _gachaData;
    [SerializeField] private GachaSetting _gachaSetting; // レア度設定
    [SerializeField] Text _loadingText; // ローディングテキストの追加

    [SerializeField] private Image _singleButton;

    [SerializeField] private Image _tenButton;

    [SerializeField] private Image _changeButton;

    [SerializeField] private Image _probabilityButton;

    // レア度ごとのシーン名
    [SerializeField] private string _normalScene = "";

    [SerializeField] private string _oneSSRScene = "";

    [SerializeField] private string _moreSSRScene = "";

    [SerializeField] private string _oneURScene = "";

    private int _maxImages = 1;

    private bool _isGachaInProgress = false;

    // 単発ガチャがクリックされたときに呼び出される
    public void OnSingleGachaClick()
    {
        if (_isGachaInProgress) return;

        _maxImages = 1; // 単発ガチャ
        StartCoroutine(GetAPI(_maxImages));
        _loadingText.gameObject.SetActive(true); // ローディングテキストを表示
        _isGachaInProgress = true;

        // ボタンを無効化する処理
        _singleButton.raycastTarget = false;
        _tenButton.raycastTarget = false;
        _changeButton.raycastTarget = false;
        _probabilityButton.raycastTarget = false;
    }

    // 10連ガチャがクリックされたときに呼び出される
    public void OnTenGachaClick()
    {
        if (_isGachaInProgress) return;

        _maxImages = 10; // 10連ガチャ
        StartCoroutine(GetAPI(_maxImages));
        _loadingText.gameObject.SetActive(true); // ローディングテキストを表示
        _isGachaInProgress = true;

        // ボタンを無効化する処理（必要に応じて）
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
        _gachaData.gachaResults = new GachaData.GachaResult[count];

        for (int i = 0; i < count; i++)
        {
            // レア度をランダムに決定
            Rarity selectedRarity = GetRandomRarity();

            UnityWebRequest request = UnityWebRequest.Get("https://dog.ceo/api/breeds/image/random");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                ResponseData response = JsonUtility.FromJson<ResponseData>(jsonResponse);
                yield return StartCoroutine(GetTexture(response.message, i, selectedRarity));

                if (i == count - 1) // 最後の画像のロードが完了した場合
                {
                    _loadingText.gameObject.SetActive(false); // ローディングテキストを非表示に
                    _isGachaInProgress = false; // ガチャの進行状態をリセット

                    // ボタンを再度有効にする処理
                     _singleButton.raycastTarget = true;
                    _tenButton.raycastTarget = true;
                    _changeButton.raycastTarget = true;
                    _probabilityButton.raycastTarget = true;
                }
            }
            else
            {
                Debug.LogError($"画像取得失敗: {request.error}");

                _loadingText.text = "ロード失敗"; // エラーメッセージに更新
            }
        }

        // ガチャ結果の取得が完了
        Debug.Log("ガチャ結果の取得が完了しました");

        // レア度のカウントを取得
        int ssrCount = _gachaData.gachaResults.Count(result => result.rarity == Rarity.SSR);
        int urCount = _gachaData.gachaResults.Count(result => result.rarity == Rarity.UR);

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
