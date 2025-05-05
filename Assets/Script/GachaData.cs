using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GachaData", menuName = "Gacha/GachaData")]
public class GachaData : ScriptableObject
{
    // �摜�ƃ��A�x���Z�b�g�ŕۑ�
    [Serializable]
    public class GachaResult
    {
        public Texture2D texture; // �摜
        public Rarity rarity;     // ���A�x
    }

    [SerializeField]
    private GachaResult[] _gachaResults;

    public GachaResult[] GachaResults
    {
        get => _gachaResults;
        set => _gachaResults = value;
    }

    [SerializeField] int _totalGachaCount = 0;  // �K�`������
    [SerializeField] int _ssrCount = 0; // SSR�̏o����
    [SerializeField] int _urCount = 0;  // UR�̏o����

    // �v���p�e�B
    public int TotalGachaCount => _totalGachaCount; // �K�`������
    public int SsrCount => _ssrCount; // SSR�̏o����
    public int UrCount => _urCount;   // UR�̏o����

    /// <summary>
    /// �������Ƃ��ɃJ�E���g�ǉ����郁�\�b�h
    /// </summary>
    public void AddGachaResult(Rarity rarity)
    {
        _totalGachaCount++;

        switch (rarity)
        {
            case Rarity.SSR:
                _ssrCount++;
                break;
            case Rarity.UR:
                _urCount++;
                break;
        }
    }

    /// <summary>
    /// �K�`�����񐔕ۑ�
    /// </summary>
    public void SaveData()
    {
        PlayerPrefs.SetInt("TotalGachaCount", _totalGachaCount);
        PlayerPrefs.SetInt("SSRCount", _ssrCount);
        PlayerPrefs.SetInt("URCount", _urCount);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// �K�`�����񐔓ǂݍ���
    /// </summary>
    public void LoadData()
    {
        _totalGachaCount = PlayerPrefs.GetInt("TotalGachaCount", 0);
        _ssrCount = PlayerPrefs.GetInt("SSRCount", 0);
        _urCount = PlayerPrefs.GetInt("URCount", 0);
    }
}

