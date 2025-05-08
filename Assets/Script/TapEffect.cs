using UnityEngine;

public class TapEffect : MonoBehaviour
{
    [SerializeField] private GameObject _particlePrefab;  // �p�[�e�B�N���v���n�u
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // �^�b�v/�N���b�N���o
        {
            Vector3 screenPos = Input.mousePosition;

            // �X�N���[�����W �� ���[���h���W�ɕϊ�
            screenPos.z = Mathf.Abs(_mainCamera.transform.position.z); // �J�����Ƃ̋�����ݒ�i2D�Ȃ�ʏ�10�j
            Vector3 worldPos = _mainCamera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;  // Z���͌Œ�i2D�Q�[���p�j

            // �p�[�e�B�N������
            GameObject particle = Instantiate(_particlePrefab, worldPos, Quaternion.identity);

            // �Đ��iPlay On Awake ���I�t�̏ꍇ�j
            ParticleSystem ps = particle.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();

            // ��莞�Ԍ�ɍ폜
            Destroy(particle, 1f);
        }
    }
}
