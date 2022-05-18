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
    TurretManager turretManager;
    public TurretManager TurretManager
    {
        get
        {
            return turretManager;
        }
    }

    [SerializeField]
    BulletManager bulletManager;
    public BulletManager BulletManager
    {
        get
        {
            return bulletManager;
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

    [SerializeField]
    GameFlowManager gameFlowManager;
    public GameFlowManager GameFlowManager
    {
        get
        {
            return gameFlowManager;
        }
    }

    [SerializeField]
    LoadJson loadJson;
    public LoadJson LoadJson
    {
        get
        {
            return loadJson;
        }
    }

    [SerializeField]
    ShaderController shaderController;
    public ShaderController ShaderController
    {
        get
        {
            return shaderController;
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
