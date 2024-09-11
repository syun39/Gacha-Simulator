using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GachaResultDisplay : MonoBehaviour
{
    // GachaData ScriptableObject の参照
    [SerializeField] GachaData _gachaData;

    // Unity UI の Image コンポーネントへの参照
    [SerializeField] Image _gachaImage;

    private int _currentImageIndex = 0;

    void Start()
    {
        // 最初の画像を表示（テクスチャが存在する場合）
        if (_gachaData.gachaTextures.Length > 0)
        {
            ApplyTextureToImage(_gachaData.gachaTextures[_currentImageIndex]);
        }
    }

    void Update()
    {
        // エンターキーが押されたら次の画像を表示
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_currentImageIndex < _gachaData.gachaTextures.Length - 1)
            {
                _currentImageIndex++;
                ApplyTextureToImage(_gachaData.gachaTextures[_currentImageIndex]);
            }
            else
            {
                // 全ての画像を表示し終わったらシーン遷移
                SceneManager.LoadScene("Gacha Result Scene"); // 最終結果を表示するシーンに遷移
            }
        }
    }

    // テクスチャをImageコンポーネントに適用するメソッド
    void ApplyTextureToImage(Texture2D texture)
    {
        // テクスチャを Sprite に変換して UI の Image に適用
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        _gachaImage.sprite = newSprite;
    }
}
