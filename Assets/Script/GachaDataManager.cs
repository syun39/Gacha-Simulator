using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaDataManager : MonoBehaviour
{
    public static GachaDataManager Instance { get; private set; }
    [SerializeField] private GachaData _gachaData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // 既に存在する場合は自分自身を破棄
        }
    }
}
