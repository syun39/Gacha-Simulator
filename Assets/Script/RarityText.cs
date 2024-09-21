using UnityEngine;

public class RarityText : MonoBehaviour
{
    [SerializeField] private GameObject _rarityTextR; // Rテキスト
    [SerializeField] private GameObject _rarityTextSR; // SRテキスト
    [SerializeField] private GameObject _rarityTextSSR; // SSRテキスト
    [SerializeField] private GameObject _rarityTextUR; // URテキスト

    /// <summary>
    /// レアリティに応じて該当するテキストを表示
    /// </summary>
    public void SetRarity(Rarity rarity)
    {
        // 全てのレアリティテキストを非表示にする
        _rarityTextR.SetActive(false);
        _rarityTextSR.SetActive(false);
        _rarityTextSSR.SetActive(false);
        _rarityTextUR.SetActive(false);

        // 選択されたレアリティのテキストを表示する
        if (rarity == Rarity.R)
        {
            _rarityTextR.SetActive(true); // Rを表示
        }
        else if (rarity == Rarity.SR)
        {
            _rarityTextSR.SetActive(true); // SRを表示
        }
        else if (rarity == Rarity.SSR)
        {
            _rarityTextSSR.SetActive(true); // SSRを表示
        }
        else if (rarity == Rarity.UR)
        {
            _rarityTextUR.SetActive(true); // URを表示
        }
    }
}
