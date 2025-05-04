using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DataReset : MonoBehaviour
{
    [SerializeField] private GameObject _resetText; // ���Z�b�g��ɕ\������e�L�X�g
    [SerializeField] private int _displayTime = 2; // �\�����ԁi�b�j
    [SerializeField] private Image _startButton; // �X�^�[�g�{�^��
    [SerializeField] private Image _resetButton; // ���Z�b�g�{�^��

    private bool _isResetInProgress = false; // ���Z�b�g�����ǂ���

    public void OnClickReset()
    {
        if (_isResetInProgress) return; // ��d���s�h�~
        _isResetInProgress = true;

        // �{�^��������
        _startButton.raycastTarget = false;
        _resetButton.raycastTarget = false;

        // �f�[�^�폜
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // ���Z�b�g��̕\���J�n
        StartCoroutine(DisplayResetText());
    }

    private IEnumerator DisplayResetText()
    {
        _resetText.SetActive(true);
        yield return new WaitForSeconds(_displayTime);
        _resetText.SetActive(false);

        // �{�^���ėL�����i�K�v�Ȃ�j
        _startButton.raycastTarget = true;
        _resetButton.raycastTarget = true;

        _isResetInProgress = false;
    }
}
