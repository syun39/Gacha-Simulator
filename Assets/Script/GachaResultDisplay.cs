using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GachaResultDisplay : MonoBehaviour
{
    // GachaData
    [SerializeField] GachaData _gachaData;
    [SerializeField] Image _gachaImage; // 画像表示
    [SerializeField] RarityText _rarityTextComponent; // レア度の表示

    [SerializeField] Text _currentImageIndexText; // 現在の画像が何枚目かを表示するテキスト

    // 現在の画像が何枚目か
    private int _currentImageIndex = 0;

    void Start()
    {
        // ガチャ結果が存在していたら
        if (_gachaData.GachaResults.Length > 0)
        {
            DisplayResult(_currentImageIndex); // 初期画像表示
        }
    }

    void Update()
    {
        // エンターキーが押されたら
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // 現在の画像インデックスが最後の画像のインデックスより小さい場合
            if (_currentImageIndex < _gachaData.GachaResults.Length - 1)
            {
                _currentImageIndex++;
                DisplayResult(_currentImageIndex); // 次の画像を表示
            }
            else
            {
                // すべての画像を表示したらシーン遷移
                SceneManager.LoadScene("Result Scene");
            }
        }
    }

    /// <summary>
    /// 画像とレア度を表示する
    /// </summary>
    /// <param name="index">表示する画像のインデックス</param>
    void DisplayResult(int index)
    {
        // インデックスの結果を取得
        var result = _gachaData.GachaResults[index];

        // テクスチャをSpriteに変更
        Sprite newSprite = Sprite.Create(result.texture, new Rect(0, 0, result.texture.width, result.texture.height), new Vector2(0.5f, 0.5f));
        _gachaImage.sprite = newSprite;

        // レア度を表示
        _rarityTextComponent.SetRarity(result.rarity);

        // 現在の画像が何枚目かを表示
        _currentImageIndexText.text = $"{_currentImageIndex + 1}";
    }
}
