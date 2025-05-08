using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    [Tooltip("遷移先のシーン名")]
    [SerializeField] private string _nextScene = null;

    [SerializeField] private string _skipScene = null; // ガチャ演出をスキップ
    [SerializeField] private float _waitSecond = 0.5f; // シーン遷移の待ち時間
    [SerializeField] private Button _clickButton = null; // クリックボタン

    [Tooltip("BGMのAudioSource"), Header("URシーンのみアタッチ")]
    [SerializeField] private AudioSource _bgmSource = null;

    [Tooltip("SEのAudioSource"), Header("URシーンのみアタッチ")]
    [SerializeField] private AudioSource _seSource = null;

    [Tooltip("SEが鳴ったら表示するパネル"), Header("URシーンのみアタッチ")]
    [SerializeField] private GameObject _panel = null;

    [Tooltip("SEが鳴ったら表示するイラスト"), Header("URシーンのみアタッチ")]
    [SerializeField] private GameObject _image = null;

    [Tooltip("確定演出時に消すボタン"), Header("URシーンのみアタッチ")]
    [SerializeField] private GameObject _skipButton = null;

    [Tooltip("エンターを押されたら表示するイラスト"), Header("SSRTwoシーンのみアタッチ")]
    [SerializeField] private GameObject _mikuRin = null;

    [Tooltip("スタートボタン"), Header("Titleシーンのみアタッチ")]
    [SerializeField] private Button _startButton = null;

    [Tooltip("タイトルボタン"), Header("Resultシーンのみアタッチ")]
    [SerializeField] private Button _titleButton = null;

    [Tooltip("セレクトシーンボタン"), Header("Resultシーンのみアタッチ")]
    [SerializeField] private Button _selectSceneButton = null;

    // URシーンかどうか
    private bool _isURScene = false;

    // SSR2枚シーンかどうか
    private bool _isSSRTwoScene = false;

    private void Start()
    {
        if (_clickButton != null)
        {
            _clickButton.interactable = true; // クリック有効
        }

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
    }

    /// <summary>
    /// クリックされたら
    /// </summary>
    public void OnClickScreen()
    {
        if (_clickButton.interactable)
        {
            _clickButton.interactable = false; // クリックを無効

            if (_isURScene)
            {
                StartCoroutine(URChangeScene());
            }
            else if (_isSSRTwoScene)
            {
                StartCoroutine(SSRTwoChangeScene());
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
            SceneManager.LoadScene(_nextScene); // シーンに遷移する
        }
    }

    /// <summary>
    /// 演出スキップ
    /// </summary>
    public void SkipScene()
    {
        if (_skipScene != null)
        {
            // シーンに遷移する
            SceneManager.LoadScene(_skipScene);
        }
    }

    /// <summary>
    /// シーン遷移を遅らせる場合
    /// </summary>
    public void WaitChangeScene()
    {
        if (_clickButton != null)
        {
            _clickButton.interactable = false; // クリックを無効
        }
        if (_startButton != null)
        {
            _startButton.interactable = false; // スタートボタンを無効
        }
        if (_titleButton != null)
        {
            _titleButton.interactable = false; // タイトルボタンを無効
        }
        if (_selectSceneButton != null)
        {
            _selectSceneButton.interactable = false; // セレクトシーンボタンを無効
        }
        StartCoroutine(WaitLoadScene(_nextScene));
    }

    /// <summary>
    /// 待機してからシーン遷移
    /// </summary>
    IEnumerator WaitLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(_waitSecond);
        SceneManager.LoadScene(sceneName); // シーンに遷移する
    }

    /// <summary>
    /// SSR2枚以上の時のシーン遷移
    /// </summary>
    IEnumerator SSRTwoChangeScene()
    {
        yield return new WaitForSeconds(0.7f); // 待機
        _mikuRin?.SetActive(true);  // イラストを表示
        yield return new WaitForSeconds(1.7f); // 待機
        ChangeScene();
    }

    /// <summary>
    /// UR1枚以上
    /// </summary>
    IEnumerator URChangeScene()
    {
        yield return new WaitForSeconds(0.7f); // 待機

        // BGM再生中なら止める
        if (_bgmSource?.isPlaying == true)
        {
            _bgmSource.Stop();
        }

        // 1秒待機
        yield return new WaitForSeconds(1.0f);

        // SEを再生
        if (_seSource != null)
        {
            _seSource.Play();
        }

        _skipButton?.SetActive(false); // ボタンを非表示

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
