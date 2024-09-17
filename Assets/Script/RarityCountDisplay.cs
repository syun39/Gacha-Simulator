using UnityEngine;
using UnityEngine.UI;

public class RarityCountDisplay : MonoBehaviour
{
    // GachaData ScriptableObject の参照
    [SerializeField] private GachaData _gachaData;

    // レア度ごとのカウントを表示する Text UI
    [SerializeField] private Text _rCountText;

    [SerializeField] private Text _srCountText;

    [SerializeField] private Text _ssrCountText;

    [SerializeField] private Text _urCountText;

    private void Start()
    {
        DisplayRarityCounts();
    }

    /// <summary>
    /// レア度ごとのカウントを表示するメソッド
    /// </summary>
    void DisplayRarityCounts()
    {
        // レア度ごとのカウントを保存するための変数
        int rCount = 0;
        int srCount = 0;
        int ssrCount = 0;
        int urCount = 0;

        // ガチャ結果をループしてレア度ごとにカウントする
        foreach (var result in _gachaData.gachaResults)
        {
            switch (result.rarity)
            {
                case Rarity.R:
                    rCount++;
                    break;
                case Rarity.SR:
                    srCount++;
                    break;
                case Rarity.SSR:
                    ssrCount++;
                    break;
                case Rarity.UR:
                    urCount++;
                    break;
            }
        }

        // Text コンポーネントに反映する
        _rCountText.text = $"{rCount} 枚";
        _srCountText.text = $"{srCount} 枚";
        _ssrCountText.text = $"{ssrCount} 枚";
        _urCountText.text = $"{urCount} 枚";
    }
}
