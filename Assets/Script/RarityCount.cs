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

    [SerializeField] Text _totalCountText; // 総回数を表示するテキスト
    [SerializeField] Text _resultSsrCountText; // SSRの総回数を表示するテキスト
    [SerializeField] Text _resultUrCountText; // URの総回数を表示するテキスト
    [SerializeField] Text _resultButtonText; // 結果表示ボタンのテキスト

    [SerializeField] GameObject _result; // 今回の結果表示
    [SerializeField] GameObject _total; // 総回数表示
    private void Start()
    {
        _gachaData.LoadData(); // データを読み込む
        ShowRarityCountNow();
    }

    /// <summary>
    /// 表示切り替えボタンが押されたとき
    /// </summary>
    public void OnSwitchDisplay()
    {
        if (_result.activeSelf == true)
        {
            ShowRarityCountTotal(); 
        }
        else
        {
            ShowRarityCountNow(); 
        }
    }

    /// <summary>
    /// 今回の表示
    /// </summary>
    private void ShowRarityCountNow()
    {
        _result.SetActive(true); // 結果表示を有効にする
        _total.SetActive(false); // 総回数表示を無効にする

        _resultButtonText.text = "総回数"; // ボタンのテキストを変更

        int nowR = 0;
        int nowSR = 0;
        int nowSSR = 0;
        int nowUr = 0;

        foreach (var result in _gachaData.GachaResults)
        {
            switch (result.rarity)
            {
                case Rarity.R: nowR++; break;
                case Rarity.SR: nowSR++; break;
                case Rarity.SSR: nowSSR++; break;
                case Rarity.UR: nowUr++; break;
            }
        }
        // 今回のガチャ表示
        _rCountText.text = $"{nowR}枚";
        _srCountText.text = $"{nowSR}枚";
        _ssrCountText.text = $"{nowSSR}枚";
        _urCountText.text = $"{nowUr}枚";
    }

    /// <summary>
    /// 総回数表示
    /// </summary>
    private void ShowRarityCountTotal()
    {
        _result.SetActive(false); // 結果表示を無効にする
        _total.SetActive(true); // 総回数表示を有効にする

        _resultButtonText.text = "結果";　// ボタンのテキストを変更

        // 総回数表示
        _totalCountText.text = $"{_gachaData.TotalGachaCount}回";
        _resultSsrCountText.text = $"{_gachaData.SsrCount}枚";
        _resultUrCountText.text = $"{_gachaData.UrCount}枚";
    }
}
