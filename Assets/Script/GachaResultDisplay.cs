using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GachaResultDisplay : MonoBehaviour
{
    // GachaData
    [SerializeField] GachaData _gachaData;
    [SerializeField] Image _gachaImage; // �摜�\��
    [SerializeField] RarityText _rarityTextComponent; // ���A�x�̕\��

    [SerializeField] Text _currentImageIndexText; // ���݂̉摜�������ڂ���\������e�L�X�g

    // ���݂̉摜�������ڂ�
    private int _currentImageIndex = 0;

    void Start()
    {
        // �K�`�����ʂ����݂��Ă�����
        if (_gachaData.GachaResults.Length > 0)
        {
            DisplayResult(_currentImageIndex); // �����摜�\��
        }
    }

    void Update()
    {
        // �N���b�N���ꂽ��
        if (Input.GetMouseButtonDown(0))
        {
            // ���݂̉摜�C���f�b�N�X���Ō�̉摜�̃C���f�b�N�X��菬�����ꍇ
            if (_currentImageIndex < _gachaData.GachaResults.Length - 1)
            {
                _currentImageIndex++;
                DisplayResult(_currentImageIndex); // ���̉摜��\��
            }
            else
            {
                // ���ׂẲ摜��\��������V�[���J��
                SceneManager.LoadScene("Result Scene");
            }
        }
    }

    /// <summary>
    /// �摜�ƃ��A�x��\������
    /// </summary>
    /// <param name="index">�\������摜�̃C���f�b�N�X</param>
    void DisplayResult(int index)
    {
        // �C���f�b�N�X�̌��ʂ��擾
        var result = _gachaData.GachaResults[index];

        // �e�N�X�`����Sprite�ɕύX
        Sprite newSprite = Sprite.Create(result.texture, new Rect(0, 0, result.texture.width, result.texture.height), new Vector2(0.5f, 0.5f));
        _gachaImage.sprite = newSprite;

        // ���A�x��\��
        _rarityTextComponent.SetRarity(result.rarity);

        // ���݂̉摜�������ڂ���\��
        _currentImageIndexText.text = $"{_currentImageIndex + 1}";
    }
}
