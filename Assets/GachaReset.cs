using UnityEngine;
using UnityEngine.UI;

public class GachaReset : MonoBehaviour
{
    [SerializeField] private Button _resetButton1;
    [SerializeField] private Button _resetButton2;
    [SerializeField] private Button _resetButton3;
    [SerializeField] private Button _resetButton4;

    private void Start()
    {
        // 各ボタンにリセットメソッドを接続
        _resetButton1.onClick.AddListener(OnResetButtonClick);
        _resetButton2.onClick.AddListener(OnResetButtonClick);
        _resetButton3.onClick.AddListener(OnResetButtonClick);
        _resetButton4.onClick.AddListener(OnResetButtonClick);
    }

    private void OnResetButtonClick()
    {
        if (GachaResultDisplay.Instance != null)
        {
            GachaResultDisplay.Instance.Reset();
        }
    }
}
