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
    // レア度ごとの排出率を管理
    [Serializable]
    public class RarityRate
    {
        public Rarity rarity;
        public float rate; // 排出率
    }

    // 排出率を設定
    [SerializeField] public RarityRate[] rarityRates =
    {
        new RarityRate { rarity = Rarity.R, rate = 70f },
        new RarityRate { rarity = Rarity.SR, rate = 26f },
        new RarityRate { rarity = Rarity.SSR, rate = 3f },
        new RarityRate { rarity = Rarity.UR, rate = 1f }
    };
}