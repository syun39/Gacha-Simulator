using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GachaData", menuName = "Gacha/GachaData")]
public class GachaData : ScriptableObject
{
    // 画像とレア度をセットで保存
    [Serializable] public class GachaResult
    {
        public Texture2D texture; // 画像
        public Rarity rarity;     // レア度
    }

    // ガチャ結果の配列
    public GachaResult[] gachaResults;

    // ガチャ総回数
    public int totalGachaCount = 0;

    /// <summary>
    /// ガチャ総回数保存
    /// </summary>
    public void SaveData()
    {
        PlayerPrefs.SetInt("TotalGachaCount", totalGachaCount);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ガチャ総回数読み込み
    /// </summary>
    public void LoadData()
    {
        totalGachaCount = PlayerPrefs.GetInt("TotalGachaCount", 0);
    }
}

