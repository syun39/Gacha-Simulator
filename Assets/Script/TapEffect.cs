using UnityEngine;

public class TapEffect : MonoBehaviour
{
    [SerializeField] private GameObject _particlePrefab;  // パーティクルプレハブ
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // タップ/クリック検出
        {
            Vector3 screenPos = Input.mousePosition;

            // スクリーン座標 → ワールド座標に変換
            screenPos.z = Mathf.Abs(_mainCamera.transform.position.z); // カメラとの距離を設定（2Dなら通常10）
            Vector3 worldPos = _mainCamera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;  // Z軸は固定（2Dゲーム用）

            // パーティクル生成
            GameObject particle = Instantiate(_particlePrefab, worldPos, Quaternion.identity);

            // 再生（Play On Awake がオフの場合）
            ParticleSystem ps = particle.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();

            // 一定時間後に削除
            Destroy(particle, 1f);
        }
    }
}
