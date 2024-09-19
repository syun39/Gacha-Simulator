using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GachaData", menuName = "Gacha/GachaData")]
public class GachaData : ScriptableObject
{
    // 画像とレア度をセットで保存する
    [Serializable] public class GachaResult
    {
        public Texture2D texture; // 画像
        public Rarity rarity;     // レア度
    }

    // ガチャ結果の配列
    public GachaResult[] gachaResults;

    // ガチャ総回数
    public int totalGachaCount = 0;

    public void SaveData()
    {
        PlayerPrefs.SetInt("TotalGachaCount", totalGachaCount);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        totalGachaCount = PlayerPrefs.GetInt("TotalGachaCount", 0);
    }
}

