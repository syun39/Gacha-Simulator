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
        public Rarity rarity; // レア度
        public float rate; // 排出率
    }

    // 排出率を設定
    [SerializeField]
    RarityRate[] rarityRates =
    {
        new RarityRate { rarity = Rarity.R, rate = 70f }, // Rの排出率
        new RarityRate { rarity = Rarity.SR, rate = 26f }, // SRの排出率
        new RarityRate { rarity = Rarity.SSR, rate = 3f }, // SSRの排出率
        new RarityRate { rarity = Rarity.UR, rate = 1f } // URの排出率
    };

    // 排出率を取得するプロパティ
    public RarityRate[] RarityRates => rarityRates;
}