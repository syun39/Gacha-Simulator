using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class URSceneChange : MonoBehaviour
{
    // BGMとSE用のAudioSource
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _seSource;

    // SEが鳴ったら表示するパネル
    [SerializeField] private GameObject _panel;

    // 遷移先のシーン名
    [SerializeField] private string _nextScene;

    private bool _isPlay = false;
    private void Start()
    {
        _panel.SetActive(false);
        _isPlay = true;
    }

    private void Update()
    {
        // エンターキーが押されたかを確認
        if (_isPlay && Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(HandleTransition());
        }
    }

    IEnumerator HandleTransition()
    {
        // BGMを止める
        if (_bgmSource.isPlaying)
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

        // パネルを表示
        _panel.SetActive(true);

        // SEが鳴り終わるまで待機
        yield return new WaitWhile(() => _seSource.isPlaying);

        // シーン遷移
        SceneManager.LoadScene(_nextScene);
    }
}
