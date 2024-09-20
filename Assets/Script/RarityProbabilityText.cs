using UnityEngine;
using UnityEngine.UI;

enum TextState
{
    None,
    RarityRatesText,
    DecisionRarityRatesText
};

public class RarityProbabilityText : MonoBehaviour
{
    [SerializeField] Text _rarityRatesText;
    [SerializeField] Text _decisionRarityRatesText;

    private TextState _currentState = TextState.None;

    private void Start()
    {
        _rarityRatesText.gameObject.SetActive(false);
        _decisionRarityRatesText.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        if (_currentState == TextState.None)
        {
            _rarityRatesText.gameObject.SetActive(true);
            _currentState = TextState.RarityRatesText;
        }
        else if (_currentState == TextState.RarityRatesText)
        {
            _rarityRatesText.gameObject.SetActive(false);
            _decisionRarityRatesText.gameObject.SetActive(true);
            _currentState = TextState.DecisionRarityRatesText;
        }
        else if (_currentState == TextState.DecisionRarityRatesText)
        {
            _rarityRatesText.gameObject.SetActive(false);
            _decisionRarityRatesText.gameObject.SetActive(false);
            _currentState = TextState.None;
        }
    }
}
