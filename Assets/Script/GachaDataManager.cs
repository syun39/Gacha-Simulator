using UnityEngine;

public class GachaDataManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static GachaDataManager Instance { get; private set; }

    // GachaData 
    [SerializeField] private GachaData _gachaData;

    private void Awake()
    {
        // インスタンスが存在しない場合
        if (Instance == null)
        {
            Instance = this; // シングルトンとして設定
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 既に存在する場合は自分自身を破棄
        }
    }
}
