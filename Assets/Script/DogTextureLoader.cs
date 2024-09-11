using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class DogTextureLoader : MonoBehaviour
{
    // 犬の画像を取得するAPI URL
    string _urlAPI = "https://dog.ceo/api/breeds/image/random/"; // 犬

    // 受信した JSON データを Unity で扱うデータにする ResponseData ベースクラス
    [Serializable]
    public class ResponseData
    {
        // 画像URLを格納
        public string message;
    }

    // 現在表示している画像のインデックス
    private int _currentImageIndex = 0;

    // 取得した画像URLを格納する配列
    private string[] _imageUrls;

    // 画像表示中かどうかを判定するフラグ
    private bool _isDisplayingImages = false;

    // 単発ガチャボタンがクリックされたら
    public void OnSingleGachaClick(PointerEventData eventData)
    {
        // 単発ガチャ
        StartCoroutine(GetAPI(1));
    }

    // 10連ガチャボタンがクリックされたら
    public void OnTenGachaClick(PointerEventData eventData)
    {
        // 10連ガチャ
        StartCoroutine(GetAPI(10));
    }


    // クリックされたら
    public void OnPointerClick(PointerEventData eventData)
    {
        // HTTP リクエストを非同期処理を待つためコルーチンとして呼び出す
        StartCoroutine("GetAPI");
    }

    // API 取得
    IEnumerator GetAPI(int count)
    {
        // 画像URLを格納する配列をリクエスト回数分初期化
        _imageUrls = new string[count];

        // 表示する画像のインデックスをリセット
        _currentImageIndex = 0;

        // 画像表示中フラグをオンにする
        _isDisplayingImages = true;

        // リクエストを複数回送信するためのループ (単発なら1回、10連なら10回)
        for (int i = 0; i < count; i++)
        {
            // APIにGETリクエストを送信
            UnityWebRequest request = UnityWebRequest.Get(_urlAPI);

            // リクエストの完了を待つ
            yield return request.SendWebRequest();

            // リクエストの結果に応じて処理を分岐
            switch (request.result)
            {
                // リクエストが進行中の場合
                case UnityWebRequest.Result.InProgress:
                    Debug.Log("リクエスト中");
                    break;

                // リクエストが成功した場合
                case UnityWebRequest.Result.Success:
                    Debug.Log("リクエスト成功");
                    ResponseData response = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);

                    // 取得した画像URLを配列に保存
                    _imageUrls[i] = response.message;

                    // 最初の画像はすぐに表示する
                    if (i == 0)
                    {
                        StartCoroutine(GetTexture(_imageUrls[_currentImageIndex]));
                    }
                    break;

                // 接続エラーの場合
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError($"リクエスト失敗: {request.error}");
                    break;
            }

            // 10連ガチャの場合リクエスト間に1秒間隔を開ける
            if (count > 1)
            {
                yield return new WaitForSeconds(1f); // 1秒待機
            }
        }

        // 全てのリクエストが完了したため、画像表示中フラグをオフにする
        _isDisplayingImages = false;
    }

    // 指定したURLからテクスチャを取得
    IEnumerator GetTexture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        // リクエストが完了するまで待機
        yield return request.SendWebRequest();

        // テクスチャの取得が成功した場合
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("画像取得成功");

            // 取得したテクスチャをシーン内のオブジェクトに適用
            Texture loadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            GameObject.Find("Tile").GetComponent<MeshRenderer>().material.SetTexture("_MainTex", loadedTexture);
        }
        else // 画像の取得が失敗した場合
        {
            
            Debug.LogError($"画像取得失敗: {request.error}");
        }
    }

    void Update()
    {
        // 画像表示中で、エンターキーが押された場合
        if (_isDisplayingImages && Input.GetKeyDown(KeyCode.Return))
        {
            // 次の画像が存在する場合、インデックスを進めて次の画像を表示
            if (_currentImageIndex < _imageUrls.Length - 1)
            {
                _currentImageIndex++;
                StartCoroutine(GetTexture(_imageUrls[_currentImageIndex])); // 次の画像を表示
            }
        }
    }
}