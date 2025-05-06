using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [Tooltip("�J�ڐ�̃V�[����")]
    [SerializeField] private string _nextScene = null;

    [SerializeField] private string _skipScene = null; // �K�`�����o���X�L�b�v
    [SerializeField] private float _waitSecond = 0.5f; // �V�[���J�ڂ̑҂�����

    [Tooltip("BGM��AudioSource"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private AudioSource _bgmSource = null;

    [Tooltip("SE��AudioSource"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private AudioSource _seSource = null;

    [Tooltip("SE��������\������p�l��"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private GameObject _panel = null;

    [Tooltip("SE��������\������C���X�g"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private GameObject _image = null;

    [Tooltip("�m�艉�o���ɏ����{�^��"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private GameObject _skipButton = null;

    [Tooltip("�G���^�[�������ꂽ��\������C���X�g"), Header("SSRTwo�V�[���̂݃A�^�b�`")]
    [SerializeField] private GameObject _mikuRin = null;

    // �N���b�N�𖳌��ɂ��邩�ǂ���
    private bool _isInvalid = false;

    // UR�V�[�����ǂ���
    private bool _isURScene = false;

    // SSR2���V�[�����ǂ���
    private bool _isSSRTwoScene = false;

    private void Start()
    {
        _isInvalid = true; // �N���b�N�L��

        // UR�V�[���Ȃ�
        if (SceneManager.GetActiveScene().name == "UR Scene")
        {
            _panel.SetActive(false);
            _image.SetActive(false);
            _isURScene = true;
        }
        else if (SceneManager.GetActiveScene().name == "SSR Two Scene") // SSRTwoScene�V�[���Ȃ�
        {
            _mikuRin.SetActive(false);
            _isSSRTwoScene = true;
        }
    }

    /// <summary>
    /// �N���b�N���ꂽ��
    /// </summary>
    public void OnClickScreen()
    {
        if (_isInvalid)
        {
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
    /// �V�[���J��
    /// </summary>
    public void ChangeScene()
    {
        if (_nextScene != null)
        {
            // �V�[���ɑJ�ڂ���
            SceneManager.LoadScene(_nextScene);
        }
    }

    /// <summary>
    /// ���o�X�L�b�v
    /// </summary>
    public void SkipScene()
    {
        if (_skipScene != null)
        {
            // �V�[���ɑJ�ڂ���
            SceneManager.LoadScene(_skipScene);
        }
    }

    /// <summary>
    /// �V�[���J�ڂ�x�点��ꍇ
    /// </summary>
    public void WaitChangeScene()
    {
        StartCoroutine(WaitLoadScene(_nextScene));
    }

    /// <summary>
    /// �ҋ@���Ă���V�[���J��
    /// </summary>
    IEnumerator WaitLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(_waitSecond);
        SceneManager.LoadScene(sceneName); // �V�[���ɑJ�ڂ���
    }

    /// <summary>
    /// SSR2���ȏ�̎��̃V�[���J��
    /// </summary>
    IEnumerator SSRTwoChangeScene()
    {
        _mikuRin?.SetActive(true);  // �C���X�g��\��
        _isInvalid = false; // �N���b�N�𖳌�
        yield return new WaitForSeconds(1.7f); // �ҋ@
        ChangeScene();
    }

    /// <summary>
    /// UR1���ȏ�
    /// </summary>
    IEnumerator URChangeScene()
    {
        // BGM�Đ����Ȃ�~�߂�
        if (_bgmSource?.isPlaying == true)
        {
            _bgmSource.Stop();
        }
        _isInvalid = false; // �N���b�N�𖳌�

        // 1�b�ҋ@
        yield return new WaitForSeconds(1.0f);

        // SE���Đ�
        if (_seSource != null)
        {
            _seSource.Play();
        }

        _skipButton?.SetActive(false); // �{�^�����\��

        // �C���X�g��\��
        _image?.SetActive(true);

        // 0.5�b�ҋ@
        yield return new WaitForSeconds(0.5f);

        // �p�l����\��
        _panel?.SetActive(true);

        //�C���X�g���\��
        _image?.SetActive(false);

        // SE����I���܂őҋ@
        yield return new WaitWhile(() => _seSource.isPlaying);

        // �V�[���J��
        ChangeScene();
    }
}
