using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [Tooltip("遷移先のシーン名")]
    [SerializeField] private string _nextScene = null;

    [Tooltip("BGMのAudioSource"), Header("URシーンのみアタッチ")]
    [SerializeField] private AudioSource _bgmSource = null;

    [Tooltip("SEのAudioSource"), Header("URシーンのみアタッチ")]
    [SerializeField] private AudioSource _seSource = null;

    [Tooltip("SEが鳴ったら表示するパネル"), Header("URシーンのみアタッチ")]
    [SerializeField] private GameObject _panel = null;

    [Tooltip("SEが鳴ったら表示するイラスト"), Header("URシーンのみアタッチ")]
    [SerializeField] private GameObject _image = null;

    [Tooltip("エンターを押されたら表示するイラスト"), Header("SSRTwoシーンのみアタッチ")]
    [SerializeField] private GameObject _mikuRin = null;

    // エンターキーを無効にするかどうか
    private bool _isInvalid = false;

    // URシーンかどうか
    private bool _isURScene = false;

    // SSRシーンかどうか
    private bool _isSSRTwoScene = false;

    // Resultシーンかどうか
    private bool _isResultScene = false;

    private void Start()
    {
        _isInvalid = true; // エンターキー有効

        // URシーンなら
        if (SceneManager.GetActiveScene().name == "UR Scene")
        {
            _panel.SetActive(false);
            _image.SetActive(false);
            _isURScene = true;
        }
        else if (SceneManager.GetActiveScene().name == "SSR Two Scene") // SSRTwoSceneシーンなら
        {
            _mikuRin.SetActive(false);
            _isSSRTwoScene = true;
        }
        else if (SceneManager.GetActiveScene().name == "Result Scene") // Resultシーンなら
        {
            _isResultScene = true;
        }
    }

    private void Update()
    {
        // エンターキーが押されたら
        if (_isInvalid && Input.GetKeyDown(KeyCode.Return))
        {
            if (_isURScene)
            {
                StartCoroutine(URChangeScene());
            }
            else if (_isSSRTwoScene)
            {
                StartCoroutine(SSRTwoChangeScene());
            }
            else if (_isResultScene)
            {
                return;
            }
            else
            {
                ChangeScene();
            }
        }
    }

    /// <summary>
    /// シーン遷移
    /// </summary>
    public void ChangeScene()
    {
        if (_nextScene != null)
        {
            // シーンに遷移する
            SceneManager.LoadScene(_nextScene);
        }

    }

    /// <summary>
    /// シーン遷移を遅らせる場合
    /// </summary>
    public void ChangeTitleScene(string sceneName)
    {
        StartCoroutine(WaitLoadScene(sceneName));
    }

    /// <summary>
    /// 待機してからシーン遷移
    /// </summary>
    IEnumerator WaitLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(sceneName); // シーンに遷移する
    }

    /// <summary>
    /// SSR2枚以上の時のシーン遷移
    /// </summary>
    IEnumerator SSRTwoChangeScene()
    {
        _mikuRin?.SetActive(true);  // イラストを表示
        _isInvalid = false; // エンターキーを無効
        yield return new WaitForSeconds(1.7f); // 待機
        ChangeScene();
    }

    /// <summary>
    /// UR1枚以上
    /// </summary>
    IEnumerator URChangeScene()
    {
        // BGM再生中なら止める
        if (_bgmSource?.isPlaying == true)
        {
            _bgmSource.Stop();
        }
        _isInvalid = false; // エンターキーを無効

        // 1秒待機
        yield return new WaitForSeconds(1.0f);

        // SEを再生
        if (_seSource != null)
        {
            _seSource.Play();
        }

        // イラストを表示
        _image?.SetActive(true);

        // 0.5秒待機
        yield return new WaitForSeconds(0.5f);

        // パネルを表示
        _panel?.SetActive(true);

        //イラストを非表示
        _image?.SetActive(false);

        // SEが鳴り終わるまで待機
        yield return new WaitWhile(() => _seSource.isPlaying);

        // シーン遷移
        ChangeScene();
    }
}
