using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class CatTextureLoader : MonoBehaviour
{
    // 猫の画像を取得するためのAPI URL
    private string _urlAPI = "https://cataas.com/cat";

    // テクスチャを適用する対象のオブジェクト
    private GameObject _object;

    void Start()
    {
        // 画像を貼り付けるオブジェクトをシーンから取得
        _object = GameObject.Find("Tile");
    }

    // クリックされたら
    public void OnPointerClick(PointerEventData eventData)
    {
        // 猫画像を取得
        StartCoroutine(GetTexture(_urlAPI));
    }

    // 指定したURLからテクスチャを取得
    IEnumerator GetTexture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        // リクエストの送信と待機
        yield return request.SendWebRequest();

        switch (request.result)
        {
            // リクエストが進行中の場合
            case UnityWebRequest.Result.InProgress:
                Debug.Log("リクエスト中");
                break;

            // リクエストが成功した場合
            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");

                // テクスチャを取得
                Texture loadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                // 取得したテクスチャをオブジェクトに適用
                _object.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", loadedTexture);
                break;

            // インターネット接続の問題によるエラー
            case UnityWebRequest.Result.ConnectionError: 
                Debug.LogError($"リクエスト失敗: {request.error}");
                break;
        }
    }
}
