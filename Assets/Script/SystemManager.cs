using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    static SystemManager instance = null;


    //싱글톤 프로퍼티
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
        //유일한 instance
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        //Scene이동간에 사라지지 않도록 처리
        DontDestroyOnLoad(gameObject);
    }


}
