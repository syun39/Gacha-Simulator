using UnityEngine;

public class GachaSelection : MonoBehaviour
{
    [SerializeField] GameObject _dogGachaCanvas; // 犬のキャンバス
    [SerializeField] GameObject _catGachaCanvas; // 猫のキャンバス

    private void Start()
    {
        ShowDogGachaCanvas(); // 初期は犬
    }

    /// <summary>
    /// 犬のガチャ Canvas を表示
    /// </summary>
    public void ShowDogGachaCanvas()
    {
        if (_dogGachaCanvas != null)
        {
            _dogGachaCanvas.SetActive(true);
        }

        if (_catGachaCanvas != null)
        {
            _catGachaCanvas.SetActive(false);
        }
    }

    /// <summary>
    /// 猫のガチャ Canvas を表示
    /// </summary>
    public void ShowCatGachaCanvas()
    {
        if (_dogGachaCanvas != null)
        {
            _dogGachaCanvas.SetActive(false);
        }

        if (_catGachaCanvas != null)
        {
            _catGachaCanvas.SetActive(true);
        }
    }
}
