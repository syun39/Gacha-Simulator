using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// テキストの状態を管理
/// </summary>
enum TextState
{
    /// <summary>
    /// 何も表示しない
    /// </summary>
    None,

    /// <summary>
    /// ガチャ確率のテキストを表示
    /// </summary>
    RarityRatesText,

    /// <summary>
    /// 確定枠の確率を表示
    /// </summary>
    DecisionRarityRatesText
};

public class RarityProbabilityText : MonoBehaviour
{
    [SerializeField] Text _rarityRatesText; // ガチャ確率のテキスト
    [SerializeField] Text _decisionRarityRatesText; // 確定枠の確率のテキスト

    private TextState _currentState = TextState.None; // 現在のテキストの状態

    private void Start()
    {
        // 全てのテキストを非表示
        _rarityRatesText.gameObject.SetActive(false);
        _decisionRarityRatesText.gameObject.SetActive(false);
    }

    /// <summary>
    /// ボタンがクリックされたら
    /// </summary>
    public void OnButtonClick()
    {
        // 何も表示されていないなら
        if (_currentState == TextState.None)
        {
            _rarityRatesText.gameObject.SetActive(true); // ガチャ確率のテキストを表示
            _currentState = TextState.RarityRatesText; // テキスト状態を更新
        }
        else if (_currentState == TextState.RarityRatesText) // ガチャ確率のテキストが表示されているなら
        {
            _rarityRatesText.gameObject.SetActive(false); // ガチャ確率のテキストを非表示
            _decisionRarityRatesText.gameObject.SetActive(true); // 確定枠の確率テキストを表示
            _currentState = TextState.DecisionRarityRatesText; // テキスト状態を更新
        }
        else if (_currentState == TextState.DecisionRarityRatesText) // 確定枠の確率が表示されているなら
        {
            _rarityRatesText.gameObject.SetActive(false);
            _decisionRarityRatesText.gameObject.SetActive(false); // 確定枠の確率のテキストを非表示
            _currentState = TextState.None; // テキスト状態を更新
        }
    }
}
