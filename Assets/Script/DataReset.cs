using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DataReset : MonoBehaviour
{
    [SerializeField] private GameObject _resetText; // リセット後に表示するテキスト
    [SerializeField] private int _displayTime = 2; // 表示時間（秒）
    [SerializeField] private Image _startButton; // スタートボタン
    [SerializeField] private Image _resetButton; // リセットボタン

    private bool _isResetInProgress = false; // リセット中かどうか

    public void OnClickReset()
    {
        if (_isResetInProgress) return; // 二重実行防止
        _isResetInProgress = true;

        // ボタン無効化
        _startButton.raycastTarget = false;
        _resetButton.raycastTarget = false;

        // データ削除
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // リセット後の表示開始
        StartCoroutine(DisplayResetText());
    }

    private IEnumerator DisplayResetText()
    {
        _resetText.SetActive(true);
        yield return new WaitForSeconds(_displayTime);
        _resetText.SetActive(false);

        // ボタン再有効化（必要なら）
        _startButton.raycastTarget = true;
        _resetButton.raycastTarget = true;

        _isResetInProgress = false;
    }
}
