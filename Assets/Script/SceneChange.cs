using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // 遷移するシーンの名前をインスペクターで設定する
    [SerializeField] private string _sceneToLoad;

    void Update()
    {
        // エンターキーが押されたかチェック
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // 指定されたシーンに遷移する
            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}
