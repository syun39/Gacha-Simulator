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

    // �K�`������
    [SerializeField] int _totalGachaCount = 0;

    // �v���p�e�B
    public int TotalGachaCount
    {
        get => _totalGachaCount;
        set => _totalGachaCount = value;
    }

    /// <summary>
    /// �K�`�����񐔕ۑ�
    /// </summary>
    public void SaveData()
    {
        PlayerPrefs.SetInt("TotalGachaCount", _totalGachaCount);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// �K�`�����񐔓ǂݍ���
    /// </summary>
    public void LoadData()
    {
        _totalGachaCount = PlayerPrefs.GetInt("TotalGachaCount", 0);
    }
}

