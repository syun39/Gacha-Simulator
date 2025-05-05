using UnityEngine;
using UnityEngine.UI;

public class RarityCount : MonoBehaviour
{
    // GachaData
    [SerializeField] GachaData _gachaData;
    [SerializeField] Text _rCountText; // R�̃J�E���g��\������e�L�X
    [SerializeField] Text _srCountText; // SR�̃J�E���g��\������e�L�X�g
    [SerializeField] Text _ssrCountText; // SSR�̃J�E���g��\������e�L�X�g
    [SerializeField] Text _urCountText; // UR�̃J�E���g��\������e�L�X�g

    [SerializeField] Text _totalCountText; // ���񐔂�\������e�L�X�g
    [SerializeField] Text _resultSsrCountText; // SSR�̑��񐔂�\������e�L�X�g
    [SerializeField] Text _resultUrCountText; // UR�̑��񐔂�\������e�L�X�g
    [SerializeField] Text _resultButtonText; // ���ʕ\���{�^���̃e�L�X�g

    [SerializeField] GameObject _result; // ����̌��ʕ\��
    [SerializeField] GameObject _total; // ���񐔕\��
    private void Start()
    {
        _gachaData.LoadData(); // �f�[�^��ǂݍ���
        ShowRarityCountNow();
    }

    /// <summary>
    /// �\���؂�ւ��{�^���������ꂽ�Ƃ�
    /// </summary>
    public void OnSwitchDisplay()
    {
        if (_result.activeSelf == true)
        {
            ShowRarityCountTotal(); 
        }
        else
        {
            ShowRarityCountNow(); 
        }
    }

    /// <summary>
    /// ����̕\��
    /// </summary>
    private void ShowRarityCountNow()
    {
        _result.SetActive(true); // ���ʕ\����L���ɂ���
        _total.SetActive(false); // ���񐔕\���𖳌��ɂ���

        _resultButtonText.text = "����"; // �{�^���̃e�L�X�g��ύX

        int nowR = 0;
        int nowSR = 0;
        int nowSSR = 0;
        int nowUr = 0;

        foreach (var result in _gachaData.GachaResults)
        {
            switch (result.rarity)
            {
                case Rarity.R: nowR++; break;
                case Rarity.SR: nowSR++; break;
                case Rarity.SSR: nowSSR++; break;
                case Rarity.UR: nowUr++; break;
            }
        }
        // ����̃K�`���\��
        _rCountText.text = $"{nowR}��";
        _srCountText.text = $"{nowSR}��";
        _ssrCountText.text = $"{nowSSR}��";
        _urCountText.text = $"{nowUr}��";
    }

    /// <summary>
    /// ���񐔕\��
    /// </summary>
    private void ShowRarityCountTotal()
    {
        _result.SetActive(false); // ���ʕ\���𖳌��ɂ���
        _total.SetActive(true); // ���񐔕\����L���ɂ���

        _resultButtonText.text = "����";�@// �{�^���̃e�L�X�g��ύX

        // ���񐔕\��
        _totalCountText.text = $"{_gachaData.TotalGachaCount}��";
        _resultSsrCountText.text = $"{_gachaData.SsrCount}��";
        _resultUrCountText.text = $"{_gachaData.UrCount}��";
    }
}
