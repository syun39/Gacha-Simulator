using UnityEngine;

public class CeilingGachaSystem : MonoBehaviour
{
    [SerializeField] private GachaSetting _gachaSetting;
    private int _totalGachaCount = 0; // 全ガチャ回数を記録する変数
    private const int GuaranteedURCount = 200; // 200連でURを確定排出

    /// <summary>
    /// ガチャを実行し、レア度を取得する
    /// </summary>
    public Rarity PerformGacha()
    {
        _totalGachaCount++;

        // 200連目でURを確定排出
        if (_totalGachaCount % GuaranteedURCount == 0)
        {
            return Rarity.UR;
        }

        // 通常の確率でガチャを行う
        float randomValue = UnityEngine.Random.Range(0f, 100f);
        float cumulativeRate = 0f;

        foreach (var rate in _gachaSetting.rarityRates)
        {
            cumulativeRate += rate.rate;
            if (randomValue <= cumulativeRate)
            {
                return rate.rarity;
            }
        }

        return Rarity.R; // 万が一何も該当しない場合は最低レアリティを返す
    }
}
