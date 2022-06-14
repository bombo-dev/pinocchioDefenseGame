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

    //LobbyScene

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

        //�̱��� Ŭ���� �ʱ�ȭ - GameScene
        /*
        prefabCacheSystem = GameObject.FindObjectOfType<PrefabCacheSystem>();
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        turretManager = GameObject.FindObjectOfType<TurretManager>();
        bulletManager = GameObject.FindObjectOfType<BulletManager>();
        blockManager = GameObject.FindObjectOfType<BlockManager>();
        inputManager = GameObject.FindObjectOfType<InputManager>();
        gameFlowManager = GameObject.FindObjectOfType<GameFlowManager>();
        resourceManager = GameObject.FindObjectOfType<ResourceManager>();
        panelManager = GameObject.FindObjectOfType<PanelManager>();
        effectManager = GameObject.FindObjectOfType<EffectManager>();
        loadJson = GameObject.FindObjectOfType<LoadJson>();
        shaderController = GameObject.FindObjectOfType<ShaderController>();
        enemyJson = GameObject.FindObjectOfType<EnemyJson>();
        turretJson = GameObject.FindObjectOfType<TurretJson>();
        */

        //�̱��� Ŭ���� �ʱ�ȭ - LobbyScene
        prefabCacheSystem = GameObject.FindObjectOfType<PrefabCacheSystem>();
        panelManager = GameObject.FindObjectOfType<PanelManager>();
    }


}
