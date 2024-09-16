using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [Tooltip("遷移先のシーン名")]
    [SerializeField] private string _nextScene;

    [Tooltip("BGMのAudioSource"), Header("URシーンのみアタッチ")]
    [SerializeField] private AudioSource _bgmSource = null;

    [Tooltip("SEのAudioSource"), Header("URシーンのみアタッチ")]
    [SerializeField] private AudioSource _seSource = null;

    [Tooltip("SEが鳴ったら表示するパネル"), Header("URシーンのみアタッチ")]
    [SerializeField] private GameObject _panel = null;

    [Tooltip("SEが鳴ったら表示するイラスト"), Header("URシーンのみアタッチ")]
    [SerializeField] private GameObject _image = null;

    [Tooltip("エンターを押されたら表示するイラスト"),Header("SSRTwoシーンのみアタッチ")]
    [SerializeField] private GameObject _mikuRin = null;

    private bool _isPlay = false;

    // URシーンかどうか
    private bool _isURScene = false;

    // SSRシーンかどうか
    private bool _isSSRTwoScene = false;

    private void Start()
    {
        _isPlay = true;

        if (SceneManager.GetActiveScene().name == "UR Scene")
        {
            _panel.SetActive(false);
            _image.SetActive(false);
            _isURScene = true;
        }
        else if (SceneManager.GetActiveScene().name == "SSR Two Scene")
        {
            _mikuRin.SetActive(false);
            _isSSRTwoScene = true;
        }
    }

    private void Update()
    {
        // エンターキーが押されたかを確認
        if (_isPlay && Input.GetKeyDown(KeyCode.Return))
        {
            if (_isURScene)
            {
                StartCoroutine(URTransition());
            }
            else if (_isSSRTwoScene)
            {
                StartCoroutine(SSRTwoTransition());
            }
            else
            {
                // 指定されたシーンに遷移する
                SceneManager.LoadScene(_nextScene);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator SSRTwoTransition()
    {
        _mikuRin.SetActive(true);
        _isPlay = false;
        yield return new WaitForSeconds(1.7f);
        SceneManager.LoadScene(_nextScene);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator URTransition()
    {
        // BGMを止める
        if (_bgmSource?.isPlaying == true)
        {
            _bgmSource.Stop();
        }
        _isPlay = false;

        // 1秒待機
        yield return new WaitForSeconds(1.0f);

        // SEを再生
        if (_seSource != null)
        {
            _seSource.Play();
        }

        _image?.SetActive(true);

        // 0.5秒待機
        yield return new WaitForSeconds(0.5f);

        // パネルを表示
        _panel?.SetActive(true);

        _image?.SetActive(false);

        // SEが鳴り終わるまで待機
        yield return new WaitWhile(() => _seSource.isPlaying);

        // シーン遷移
        SceneManager.LoadScene(_nextScene);
    }
}
