using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GachaResultDisplay : MonoBehaviour
{
    // GachaData ScriptableObject の参照
    [SerializeField] GachaData _gachaData;

    // Unity UI の Image と Text コンポーネントへの参照
    [SerializeField] Image _gachaImage;
    [SerializeField] private RarityText _rarityTextComponent; // RarityText スクリプトの参照

    private int _currentImageIndex = 0;

    void Start()
    {
        // 最初の画像を表示（テクスチャが存在する場合）
        if (_gachaData.gachaResults.Length > 0)
        {
            DisplayResult(_currentImageIndex);
        }
    }

    void Update()
    {
        // エンターキーが押されたら次の画像を表示
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_currentImageIndex < _gachaData.gachaResults.Length - 1)
            {
                _currentImageIndex++;
                DisplayResult(_currentImageIndex);
            }
            else
            {
                // 全ての画像を表示し終わったらシーン遷移
                SceneManager.LoadScene("Gacha Select Scene"); 
            }
        }
    }

    /// <summary>
    /// 画像とレア度を表示するメソッド
    /// </summary>
    /// <param name="index"></param>
    void DisplayResult(int index)
    {
        var result = _gachaData.gachaResults[index];

        // テクスチャをImageに適用
        Sprite newSprite = Sprite.Create(result.texture, new Rect(0, 0, result.texture.width, result.texture.height), new Vector2(0.5f, 0.5f));
        _gachaImage.sprite = newSprite;

        // レア度をRarityText コンポーネントに適用
        _rarityTextComponent.SetRarity(result.rarity);
    }
}
