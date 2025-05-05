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

    [SerializeField] int _totalGachaCount = 0;  // ガチャ総回数
    [SerializeField] int _ssrCount = 0; // SSRの出現回数
    [SerializeField] int _urCount = 0;  // URの出現回数

    // プロパティ
    public int TotalGachaCount => _totalGachaCount; // ガチャ総回数
    public int SsrCount => _ssrCount; // SSRの出現回数
    public int UrCount => _urCount;   // URの出現回数

    /// <summary>
    /// 引いたときにカウント追加するメソッド
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
    /// ガチャ総回数保存
    /// </summary>
    public void SaveData()
    {
        PlayerPrefs.SetInt("TotalGachaCount", _totalGachaCount);
        PlayerPrefs.SetInt("SSRCount", _ssrCount);
        PlayerPrefs.SetInt("URCount", _urCount);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ガチャ総回数読み込み
    /// </summary>
    public void LoadData()
    {
        _totalGachaCount = PlayerPrefs.GetInt("TotalGachaCount", 0);
        _ssrCount = PlayerPrefs.GetInt("SSRCount", 0);
        _urCount = PlayerPrefs.GetInt("URCount", 0);
    }
}

