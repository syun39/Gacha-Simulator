using System;
using UnityEngine;

/// <summary>
/// レア度を定義
/// </summary>
public enum Rarity
{
    R,   // レア
    SR,  // スーパーレア
    SSR, // スーパースペシャルレア
    UR   // ウルトラレア
}

[CreateAssetMenu(fileName = "GachaSetting", menuName = "Gacha/GachaSetting")]
public class GachaSetting : ScriptableObject
{
    // レア度ごとの排出率を管理するクラス
    [Serializable]
    public class RarityRate
    {
        public Rarity rarity;
        public float rate; // 排出率
    }

    // RarityRate の配列を使って排出率を設定
    [SerializeField]
    public RarityRate[] rarityRates = new RarityRate[]
    {
        new RarityRate { rarity = Rarity.R, rate = 70f },
        new RarityRate { rarity = Rarity.SR, rate = 26.5f },
        new RarityRate { rarity = Rarity.SSR, rate = 3f },
        new RarityRate { rarity = Rarity.UR, rate = 0.5f }
    };
}