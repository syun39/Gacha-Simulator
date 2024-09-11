using UnityEngine;

[CreateAssetMenu(fileName = "GachaData", menuName = "Gacha/GachaData")]
public class GachaData : ScriptableObject
{
    // 取得したテクスチャを保存するための変数
    public Texture2D[] gachaTextures;
}