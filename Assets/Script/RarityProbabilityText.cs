using UnityEngine;
using UnityEngine.UI;

public class DogRarityProbabilityText : MonoBehaviour
{
    [SerializeField] private Text _rarityRatesText;

    private void Start()
    {
        _rarityRatesText.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        // ƒgƒOƒ‹•\Ž¦
        _rarityRatesText.gameObject.SetActive(!_rarityRatesText.gameObject.activeSelf);
    }
}
