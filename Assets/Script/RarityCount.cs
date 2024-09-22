using UnityEngine;
using UnityEngine.UI;

public class RarityCount : MonoBehaviour
{
    // GachaData
    [SerializeField] GachaData _gachaData;
    [SerializeField] Text _rCountText; // Rのカウントを表示するテキス
    [SerializeField] Text _srCountText; // SRのカウントを表示するテキスト
    [SerializeField] Text _ssrCountText; // SSRのカウントを表示するテキスト
    [SerializeField] Text _urCountText; // URのカウントを表示するテキスト

    private void Start()
    {
        RarityCounts();
    }

    /// <summary>
    /// レア度ごとのカウントを表示するメソッド
    /// </summary>
    void RarityCounts()
    {
        // レア度ごとのカウントを保存
        int rCount = 0;
        int srCount = 0;
        int ssrCount = 0;
        int urCount = 0;

        // ガチャ結果をループしてレア度ごとにカウントする
        foreach (var result in _gachaData.GachaResults)
        {
            if (result.rarity == Rarity.R) // レア度がRだったら
            {
                rCount++; // カウントを増やす
            }
            else if (result.rarity == Rarity.SR) // レア度がSRだったら
            {
                srCount++;
            }
            else if (result.rarity == Rarity.SSR) // レア度がSSRだったら
            {
                ssrCount++;
            }
            else if (result.rarity == Rarity.UR) // レア度がURだったら
            {
                urCount++;
            }
        }

        // Textで表示
        _rCountText.text = $"{rCount} 枚";
        _srCountText.text = $"{srCount} 枚";
        _ssrCountText.text = $"{ssrCount} 枚";
        _urCountText.text = $"{urCount} 枚";
    }
}
