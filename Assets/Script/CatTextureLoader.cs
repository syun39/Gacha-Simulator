using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CatTextureLoader : MonoBehaviour
{
    // 猫の画像を取得するためのAPI URL
    private string _urlAPI = "https://cataas.com/cat";

    // GachaData ScriptableObject の参照
    [SerializeField] GachaData _gachaData;

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
        // GachaData 内のテクスチャ配列を初期化
        _gachaData.gachaTextures = new Texture2D[count];

        for (int i = 0; i < count; i++)
        {
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

                // 取得したテクスチャを GachaData に保存
                _gachaData.gachaTextures[i] = DownloadHandlerTexture.GetContent(request);
            }
            else
            {
                Debug.LogError($"リクエスト失敗: {request.error}");
            }
        }

        // ガチャ結果の取得が完了
        Debug.Log("ガチャ結果の取得が完了しました");

        // ガチャが終わったらシーン遷移
        SceneManager.LoadScene("Gacha Main Scene"); // 画像表示シーンに遷移
    }
}
