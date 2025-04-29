using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [Tooltip("�J�ڐ�̃V�[����")]
    [SerializeField] private string _nextScene = null;

    // �K�`�����o���X�L�b�v
    [SerializeField] private string _skipScene = null;

    [Tooltip("BGM��AudioSource"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private AudioSource _bgmSource = null;

    [Tooltip("SE��AudioSource"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private AudioSource _seSource = null;

    [Tooltip("SE��������\������p�l��"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private GameObject _panel = null;

    [Tooltip("SE��������\������C���X�g"), Header("UR�V�[���̂݃A�^�b�`")]
    [SerializeField] private GameObject _image = null;

    [Tooltip("�G���^�[�������ꂽ��\������C���X�g"), Header("SSRTwo�V�[���̂݃A�^�b�`")]
    [SerializeField] private GameObject _mikuRin = null;

    // �G���^�[�L�[�𖳌��ɂ��邩�ǂ���
    private bool _isInvalid = false;

    // UR�V�[�����ǂ���
    private bool _isURScene = false;

    // SSR2���V�[�����ǂ���
    private bool _isSSRTwoScene = false;

    // SSR�V�[�����ǂ���
    private bool _isSSRScene = false;

    // Result�V�[�����ǂ���
    //private bool _isResultScene = false;

    // �����V�[�����ǂ���
    private bool _isNormalScene = false; 

    private void Start()
    {
        _isInvalid = true; // �G���^�[�L�[�L��

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
        //else if (SceneManager.GetActiveScene().name == "Result Scene") // Result�V�[���Ȃ�
        //{
        //    _isResultScene = true;
        //}
        else if (SceneManager.GetActiveScene().name == "Normal Scene" || 
                 SceneManager.GetActiveScene().name == "SSR Scene")// �����V�[����SSR�V�[���Ȃ�
        {
            _isNormalScene = true;
            _isSSRScene = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SkipScene();
        }

        // �G���^�[�L�[�������ꂽ��
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
            //else if (_isResultScene)
            //{
            //    return;
            //}
            else if (_isNormalScene)
            {
                ChangeScene();
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
    public void ChangeTitleScene(string sceneName)
    {
        StartCoroutine(WaitLoadScene(sceneName));
    }

    /// <summary>
    /// �ҋ@���Ă���V�[���J��
    /// </summary>
    IEnumerator WaitLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(sceneName); // �V�[���ɑJ�ڂ���
    }

    /// <summary>
    /// SSR2���ȏ�̎��̃V�[���J��
    /// </summary>
    IEnumerator SSRTwoChangeScene()
    {
        _mikuRin?.SetActive(true);  // �C���X�g��\��
        _isInvalid = false; // �G���^�[�L�[�𖳌�
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
        _isInvalid = false; // �G���^�[�L�[�𖳌�

        // 1�b�ҋ@
        yield return new WaitForSeconds(1.0f);

        // SE���Đ�
        if (_seSource != null)
        {
            _seSource.Play();
        }

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
