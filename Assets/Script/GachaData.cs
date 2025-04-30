using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GachaData", menuName = "Gacha/GachaData")]
public class GachaData : ScriptableObject
{
    // 画像とレア度をセットで保存
    [Serializable]
    public class GachaResult
    {
        public Texture2D texture; // 画像
        public Rarity rarity;     // レア度
    }

    [SerializeField]
    private GachaResult[] _gachaResults;

    public GachaResult[] GachaResults
    {
        get => _gachaResults;
        set => _gachaResults = value;
    }

    // ガチャ総回数
    [SerializeField] int _totalGachaCount = 0;

    // プロパティ
    public int TotalGachaCount
    {
        get => _totalGachaCount;
        set => _totalGachaCount = value;
    }

    /// <summary>
    /// ガチャ総回数保存
    /// </summary>
    public void SaveData()
    {
        PlayerPrefs.SetInt("TotalGachaCount", _totalGachaCount);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ガチャ総回数読み込み
    /// </summary>
    public void LoadData()
    {
        _totalGachaCount = PlayerPrefs.GetInt("TotalGachaCount", 0);
    }
}

