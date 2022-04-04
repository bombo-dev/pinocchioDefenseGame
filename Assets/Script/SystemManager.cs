using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    static SystemManager instance = null;


    //�̱��� ������Ƽ
    public static SystemManager Instance 
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    PrefabCacheSystem prefabCacheSystem;
    public PrefabCacheSystem PrefabCacheSystem
    {
        get
        {
            return prefabCacheSystem;
        }
    }

    [SerializeField]
    EnemyManager enemyManager;
    public EnemyManager EnemyManager
    {
        get
        {
            return enemyManager;
        }
    }

    [SerializeField]
    TileManager tileManager;
    public TileManager TileManager
    {
        get
        {
            return tileManager;
        }
    }

    private void Awake()
    {
        //������ instance
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        //Scene�̵����� ������� �ʵ��� ó��
        DontDestroyOnLoad(gameObject);
    }


}
