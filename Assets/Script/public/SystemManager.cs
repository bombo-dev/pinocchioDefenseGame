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

    //GameScene

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
    BlockManager blockManager;
    public BlockManager BlockManager
    {
        get
        {
            return blockManager;
        }
    }

    [SerializeField]
    InputManager inputManager;
    public InputManager InputManager
    {
        get
        {
            return inputManager;
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
    ResourceManager resourceManager;
    public ResourceManager ResourceManager
    {
        get
        {
            return resourceManager;
        }
    }

    [SerializeField]
    PanelManager panelManager;
    public PanelManager PanelManager
    {
        get
        {
            return panelManager;
        }
    }

    [SerializeField]
    EffectManager effectManager;
    public EffectManager EffectManager
    {
        get
        {
            return effectManager;
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

    [SerializeField]
    EnemyJson enemyJson;
    public EnemyJson EnemyJson
    {
        get
        {
            return enemyJson;
        }
    }

    [SerializeField]
    TurretJson turretJson;
    public TurretJson TurretJson
    {
        get
        {
            return turretJson;
        }
    }

    [SerializeField]
    UserInfo userInfo;
    public UserInfo UserInfo
    {
        get
        {
            return userInfo;
        }
    }

    //LobbyScene

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

        //싱글톤 클래스 초기화
        if (GameObject.FindObjectOfType<PrefabCacheSystem>())
            prefabCacheSystem = GameObject.FindObjectOfType<PrefabCacheSystem>();
        if (GameObject.FindObjectOfType<EnemyManager>())
            enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        if (GameObject.FindObjectOfType<TurretManager>())
            turretManager = GameObject.FindObjectOfType<TurretManager>();
        if (GameObject.FindObjectOfType<BulletManager>())
            bulletManager = GameObject.FindObjectOfType<BulletManager>();
        if (GameObject.FindObjectOfType<BlockManager>())
            blockManager = GameObject.FindObjectOfType<BlockManager>();
        if (GameObject.FindObjectOfType<InputManager>())
            inputManager = GameObject.FindObjectOfType<InputManager>();
        if (GameObject.FindObjectOfType<GameFlowManager>())
            gameFlowManager = GameObject.FindObjectOfType<GameFlowManager>();
        if (GameObject.FindObjectOfType<ResourceManager>())
            resourceManager = GameObject.FindObjectOfType<ResourceManager>();
        if (GameObject.FindObjectOfType<PanelManager>())
            panelManager = GameObject.FindObjectOfType<PanelManager>();
        if (GameObject.FindObjectOfType<EffectManager>())
            effectManager = GameObject.FindObjectOfType<EffectManager>();
        if (GameObject.FindObjectOfType<LoadJson>())
            loadJson = GameObject.FindObjectOfType<LoadJson>();
        if (GameObject.FindObjectOfType<ShaderController>())
            shaderController = GameObject.FindObjectOfType<ShaderController>();
        if (GameObject.FindObjectOfType<EnemyJson>())
            enemyJson = GameObject.FindObjectOfType<EnemyJson>();
        if (GameObject.FindObjectOfType<TurretJson>())
            turretJson = GameObject.FindObjectOfType<TurretJson>();
        if (GameObject.FindObjectOfType<UserInfo>())
            userInfo = GameObject.FindObjectOfType<UserInfo>();

    }


}
