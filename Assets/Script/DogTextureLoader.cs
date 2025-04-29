using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DogTextureLoader : MonoBehaviour
{
    // JSON �����M����f�[�^
    [Serializable]
    public class ResponseData
    {
        public string message; // �摜URL
    }

    [SerializeField] GachaData _gachaData; // GachaData

    [SerializeField] GachaSetting _gachaSetting; // �r�o��

    [Tooltip("�K�`���̓V��")]
    [SerializeField] int _ceilingCount = 200; // �V��

    [SerializeField] Text _loadingText; // ���[�f�B���O�e�L�X�g

    [SerializeField] Image _singleButton; // �P���K�`���{�^��

    [SerializeField] Image _tenButton; // 10�A�K�`���{�^��

    [SerializeField] Image _changeButton; // �K�`���؂�ւ��{�^��

    [SerializeField] Image _probabilityButton; // �m���\���{�^��

    [SerializeField] Text _remainingText; // �V��܂Ŏc�艽��

    [SerializeField] Text _text; // UR�m��܂łƕ\��

    // ���A�x���Ƃ̃V�[����
    [SerializeField] string _normalScene = ""; // �����V�[��

    [SerializeField] string _oneSSRScene = ""; // SSR�m��V�[��

    [SerializeField] string _moreSSRScene = ""; // SSR2���ȏ�V�[��

    [SerializeField] string _oneURScene = ""; // UR�m��V�[��

    private bool _isGachaInProgress = false; // �K�`�����i�s����

    private void Start()
    {
        // �Q�[���J�n���Ƀf�[�^��ǂݍ���
        _gachaData.LoadData();

        // �c��񐔂̕\�����X�V
        UpdateDogRemainingCount();
    }

    /// <summary>
    /// �P���K�`��
    /// </summary>
    public void OnSingleGachaClick()
    {
        // �K�`�����i�s���̏ꍇ
        if (_isGachaInProgress) return;

        _remainingText.gameObject.SetActive(false); // �V��܂Ŏc�艽�񂩂��\��
        _text.gameObject.SetActive(false); //UR�m��܂ł��\��
        StartCoroutine(GetAPI(1)); // 1����s
        _isGachaInProgress = true; // �K�`�����i�s��

        // �{�^���𖳌������鏈��
        _singleButton.raycastTarget = false;
        _tenButton.raycastTarget = false;
        _changeButton.raycastTarget = false;
        _probabilityButton.raycastTarget = false;
    }

    /// <summary>
    /// 10�A�K�`��
    /// </summary>
    public void OnTenGachaClick()
    {
        // �K�`�����i�s���̏ꍇ
        if (_isGachaInProgress) return;

        _remainingText.gameObject.SetActive(false); // �V��܂Ŏc�艽�񂩂��\��
        _text.gameObject.SetActive(false); //UR�m��܂ł��\��
        StartCoroutine(GetAPI(10)); // 10����s
        _isGachaInProgress = true; // �K�`�����i�s��

        // �{�^���𖳌������鏈��
        _singleButton.raycastTarget = false;
        _tenButton.raycastTarget = false;
        _changeButton.raycastTarget = false;
        _probabilityButton.raycastTarget = false;
    }

    // API���g���ĉ摜���擾���A���A�x�ƈꏏ��GachaData�ɕۑ�
    IEnumerator GetAPI(int count)
    {
        _loadingText.gameObject.SetActive(true); // ���[�f�B���O�e�L�X�g��\��

        // GachaData ���̌��ʔz���������
        _gachaData.GachaResults = new GachaData.GachaResult[count];

        for (int i = 0; i < count; i++)
        {
            Rarity selectedRarity; // ���A�x�I��

            // ���A�x�������_���Ɍ���
            if (_gachaData.TotalGachaCount % _ceilingCount == _ceilingCount - 1) // �]�肪��v�����Ƃ�
            {
                // �V��̏ꍇ�͕K��UR���o��
                selectedRarity = Rarity.UR;
            }
            else if (count == 10 && i == count - 1) // 10�A�K�`���̍Ō��SR�ȏ�
            {
                float randomValue = UnityEngine.Random.Range(0f, 100f);
                if (randomValue < 1) // 1%��UR
                {
                    selectedRarity = Rarity.UR;
                }
                else if (randomValue < 4) // 3%��SSR
                {
                    selectedRarity = Rarity.SSR;
                }
                else // �c���96%��SR
                {
                    selectedRarity = Rarity.SR;
                }
            }
            else
            {
                // �ʏ�̃K�`���Ń��A�x�������_���Ɍ���
                selectedRarity = GetRandomRarity();
            }

            // API���N�G�X�g�𑗐M
            UnityWebRequest request = UnityWebRequest.Get("https://dog.ceo/api/breeds/image/random");
            yield return request.SendWebRequest();

            // ����������
            if (request.result == UnityWebRequest.Result.Success)
            {
                // ���X�|���X(����)�̏���
                string jsonResponse = request.downloadHandler.text;
                ResponseData response = JsonUtility.FromJson<ResponseData>(jsonResponse);
                yield return StartCoroutine(GetTexture(response.message, i, selectedRarity));

                if (i == count - 1) // �Ō�̉摜�̃��[�h�����������ꍇ
                {
                    _loadingText.gameObject.SetActive(false); // ���[�f�B���O�e�L�X�g���\����
                    _isGachaInProgress = false; // �K�`���̐i�s��Ԃ����Z�b�g
                    _remainingText.gameObject.SetActive(true);
                    _text.gameObject.SetActive(true);

                    // �{�^�����ēx�L���ɂ��鏈��
                    _singleButton.raycastTarget = true;
                    _tenButton.raycastTarget = true;
                    _changeButton.raycastTarget = true;
                    _probabilityButton.raycastTarget = true;
                }
            }
            else
            {
                //Debug.LogError($"�摜�擾���s: {request.error}");

                Animator animator = _loadingText.GetComponent<Animator>(); // �e�L�X�g�̃A�j���[�^�[���擾

                if (animator != null)
                {
                    animator.enabled = false;  // �A�j���[�V�������ꎞ�I�ɖ�����
                }
                _loadingText.color = Color.red;
                _loadingText.text = "���[�h���s"; // �G���[���b�Z�[�W�ɍX�V

                yield return new WaitForSeconds(1.5f);

                // �^�C�g���ɖ߂�
                SceneManager.LoadScene("Title");
            }
        }

        // �K�`�����ʂ̎擾������
        //Debug.Log("�K�`�����ʂ̎擾���������܂���");

        // �K�`���񐔂��C���N�������g
        _gachaData.TotalGachaCount += count;

        _gachaData.SaveData(); // �f�[�^��ۑ�

        // ���A�x�̃J�E���g
        int ssrCount = 0;
        int urCount = 0;

        // �K�`�����ʂ����[�v���ă��A�x���J�E���g
        foreach (var result in _gachaData.GachaResults)
        {
            if (result.rarity == Rarity.SSR)
            {
                ssrCount++; // SSR�̏ꍇ�J�E���g�𑝂₷
            }
            else if (result.rarity == Rarity.UR)
            {
                urCount++; // UR�̏ꍇ�J�E���g�𑝂₷
            }
        }

        // �J�ڐ�̃V�[��������
        string sceneToLoad = RarityChangeScene(ssrCount, urCount);
        SceneManager.LoadScene(sceneToLoad);
    }

    // �e�N�X�`�����擾���� GachaData �ɕۑ�
    IEnumerator GetTexture(string url, int index, Rarity rarity)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);

            // GachaData �ɉ摜�ƃ��A�x��ۑ�
            _gachaData.GachaResults[index] = new GachaData.GachaResult
            {
                texture = texture,
                rarity = rarity
            };

            //Debug.Log($"Image Width: {texture.width}");
            //Debug.Log($"Image Height: {texture.height}");

        }
        else
        {
            //Debug.LogError($"�e�N�X�`���擾���s: {request.error}");

            Animator animator = _loadingText.GetComponent<Animator>(); // �e�L�X�g�̃A�j���[�^�[���擾

            if (animator != null)
            {
                animator.enabled = false;  // �A�j���[�V�������ꎞ�I�ɖ�����
            }
            _loadingText.color = Color.red;
            _loadingText.text = "�e�N�X�`���擾���s"; // �G���[���b�Z�[�W�ɍX�V

            yield return new WaitForSeconds(1.5f);

            // �^�C�g���ɖ߂�
            SceneManager.LoadScene("Title");
        }

    }

    /// <summary>
    /// ���A�x�ɉ������V�[���������肷��
    /// </summary>
    /// <param name="ssrCount">SSR�̖���</param>
    /// <param name="urCount">UR�̖���</param>
    /// <returns>�V�[����</returns>
    string RarityChangeScene(int ssrCount, int urCount)
    {
        if (urCount > 0)
        {
            return _oneURScene; // UR���ꖇ�ȏ�̏ꍇ
        }
        else if (ssrCount >= 2)
        {
            return _moreSSRScene; // SSR���񖇈ȏ�̏ꍇ
        }
        else if (ssrCount == 1)
        {
            return _oneSSRScene; // SSR���ꖇ�̏ꍇ
        }
        else
        {
            return _normalScene; // SSR��UR���Ȃ��ꍇ
        }
    }

    /// <summary>
    /// ���A�x�������_���Ɍ���(�d�ݕt���̊m�����I)
    /// </summary>
    Rarity GetRandomRarity()
    {
        float total = 0f;
        foreach (var rate in _gachaSetting.RarityRates)
        {
            total += rate.rate; // �e���A�x�̊m�������v����
        }

        float randomValue = UnityEngine.Random.Range(0, total); // �����_���Ȓl�𐶐�
        float cumulative = 0f; // �m���̗ݐϒl��ێ�

        foreach (var rate in _gachaSetting.RarityRates)
        {
            cumulative += rate.rate; // �m���̗ݐϒl���v�Z
            // �����_���Ȓl���ݐϊm���ȉ��Ȃ�
            if (randomValue <= cumulative)
            {
                return rate.rarity; // �����_���Ȓl�ɑΉ����郌�A�x��Ԃ�
            }
        }
        // R��0����70
        // SR��70.1����96
        // SSR��96.1����99
        // UR��99.1����100
        return Rarity.R; // �f�t�H���g�� R
    }

    /// <summary>
    /// �V��܂Ŏc��̃K�`���񐔂��X�V
    /// </summary>
    void UpdateDogRemainingCount()
    {
        // ���݂̃K�`���񐔂����߂� _ceilingCount - �]��
        int remainingToUR = _ceilingCount - (_gachaData.TotalGachaCount % _ceilingCount);
        _remainingText.text = $"�c�� {remainingToUR}��";
    }
}
