using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PanelManager : MonoBehaviour
{
    public static PanelManager instance = null;

    public static PanelManager Instance
    {
        get
        {
            return instance;
        }
    }

    // 활성화된 damage 패널을 저장할 리스트
    public List<GameObject> damagePanels;



    [Header("PanelCachesInfo")]
    //Load한 Panel 프리팹 정보
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    // 활성화된 panel를 받아올 리스트
    public UI_TurretMngPanel turretMngPanel;
    public UI_TurretInfoPanel turretInfoPanel;
    public StageMngPanel stageMngPanel;
    public StatusMngPanel statusMngPanel;
    public DamageMngPanel damageMngPanel;
    public UI_ResourcePanel resoursePanel;
    public UI_OptionPanel optionPanel;
    public GoodsMngPanel goodsMngPanel;
    public UI_StageEndPanel stageEndPanel;


    [SerializeField]
    Transform canvas;

    //filePath, cacheCount 저장
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;
 


    // Start is called before the first frame update
    void Start()
    {
        PrepareData();

        //바로 생성해야할 패널 생성
        EnableFixedPanel();
    }

    /// <summary>
    /// 씬 로드 후 Enemy 캐시 데이터를 바탕으로 생성할 함수 호출 : 김현진
    /// </summary>
    void PrepareData()
    {
        for (int i = 0; i < prefabCacheDatas.Length; i++)
        {
            SystemManager.Instance.PrefabCacheSystem.GeneratePrefabCache(prefabCacheDatas[i].filePath, prefabCacheDatas[i].cacheCount, Load(prefabCacheDatas[i].filePath), canvas);
        }
    }

    /// <summary>
    /// 게임 시작 부터 바로 나타나 있는 고정된 패널 생성 : 김현진
    /// </summary>
    void EnableFixedPanel()
    {
        //TurretMngPanel
        EnablePanel<UI_TurretMngPanel>(0);

        //TurretInfoPanel
        EnablePanel<UI_TurretInfoPanel>(1);
        if(turretInfoPanel)
            turretInfoPanel.Reset();

        //ResourceManaerPanel
        EnablePanel<UI_ResourcePanel>(4);

        //OptionPanel
        EnablePanel<UI_OptionPanel>(7);
    }

    /// <summary>
    /// 프리팹 경로를 통해 게임오브젝트를 가져온다 : 김현진
    /// </summary>
    /// <param name="filePath">프리팹이 저장되있는 경로</param>
    /// <returns>경로에서 가져온 게임 오브젝트</returns>
    GameObject Load(string filePath)
    {
        //이미 캐시에 포함되어 있을 경우
        if (prefabCaChes.ContainsKey(filePath))
            return prefabCaChes[filePath];
        else
        {
            GameObject go = Resources.Load<GameObject>(filePath);
            prefabCaChes.Add(filePath, go);

            return go;
        }
    }

    /// <summary>
    ///  Panel 객체를 생성 : 김현진
    /// </summary>
    /// <typeparam name="T">패널이 가지고 있는 UI_Panel 스크립트</typeparam>
    /// <param name="panelIndex">생성할 패널 번호</param>
    /// /// <param name="_gameobject">패널에 게임오브젝트 정보를 연결해야 할 경우 사용</param>
    public GameObject EnablePanel<T>(int panelIndex, GameObject _gameobject = null) where T : UnityEngine.Component
    {
        
        //예외처리
        if (panelIndex >= prefabCacheDatas.Length || prefabCacheDatas[panelIndex].filePath == null)
            return null;
       
        //생성한 프리팹 게임오브젝트 정보 받아오기
        GameObject go = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[panelIndex].filePath);

        if (go == null)
            return null;


        Vector3 screenPos;
        Enemy enemy;
        Turret turret;

        T compoenent = go.GetComponent<T>();

        if (typeof(T) == typeof(UI_TurretMngPanel))
        {
            turretMngPanel = (compoenent as UI_TurretMngPanel);
            go.transform.SetAsLastSibling();
        }
        else if (typeof(T) == typeof(UI_TurretInfoPanel))
        {
            turretInfoPanel = (compoenent as UI_TurretInfoPanel);
            {
                (compoenent as UI_TurretInfoPanel).Reset();
                go.transform.SetAsLastSibling();
            }
        }
        else if (typeof(T) == typeof(StageMngPanel))
        {
            stageMngPanel = (compoenent as StageMngPanel);

        }
        else if (typeof(T) == typeof(DamageMngPanel))
        {
            damageMngPanel = (compoenent as DamageMngPanel);
            // hpBar 패널 위치 초기화   
            damagePanels.Add(go);

            Vector3 panelPos;
            if (_gameobject.tag == "Enemy")
            {
                enemy = _gameobject.GetComponent<Enemy>();
                panelPos = enemy.hitPos.transform.position;
            }
            else if (_gameobject.tag == "Turret")
            {
                turret = _gameobject.GetComponent<Turret>();
                panelPos = turret.hitPos.transform.position;
            }
            else
                return null;

            screenPos = Camera.main.WorldToScreenPoint(panelPos);
            go.transform.position = screenPos;
        }

        else if (typeof(T) == typeof(UI_ResourcePanel))
        {
            resoursePanel = (compoenent as UI_ResourcePanel);
        }
        else if (typeof(T) == typeof(UI_ConstructionGauge))
        {
            UI_ConstructionGauge constructionGaguePanel = (compoenent as UI_ConstructionGauge);

            //예외처리
            if (!_gameobject)
                return null;

            if (constructionGaguePanel)
            {
                //패널의 게임오브젝트 정보 저장
                constructionGaguePanel.constructionTurret = _gameobject.GetComponent<ConstructionTurret>();

                //건설 게이지 패널 위치 설정
                screenPos = Camera.main.WorldToScreenPoint(constructionGaguePanel.constructionTurret.gauegePos.transform.position);
                go.transform.position = screenPos;
            }
        }
        else if (typeof(T) == typeof(StatusMngPanel))
        {
            statusMngPanel = (compoenent as StatusMngPanel);
            
            Vector3 panelPos;
            if (_gameobject.tag == "Enemy")
            {
                enemy = _gameobject.GetComponent<Enemy>();


                // hpBar가 겹쳐서 안보이는 문제를 해결하기 위해 y축 위치에 랜덤 난수 더해주기
                System.Random randNum = new System.Random();
                Debug.Log("난수 생성=" + randNum.Next(0, 10));
                panelPos = new Vector3(enemy.hpPos.transform.position.x, enemy.hpPos.transform.position.y + randNum.Next(0, 10), enemy.hpPos.transform.position.z);           

            }
            else if (_gameobject.tag == "Turret")
            {
                turret = _gameobject.GetComponent<Turret>();
                panelPos = turret.hpPos.transform.position;
            }
            else
                return null;
            screenPos = Camera.main.WorldToScreenPoint(panelPos);
            go.transform.position = screenPos;
        }
        else if (typeof(T) == typeof(UI_OptionPanel))
        {
            optionPanel = (compoenent as UI_OptionPanel);
        }
        else if (typeof(T) == typeof(GoodsMngPanel))
        {
            goodsMngPanel = (compoenent as GoodsMngPanel);
        }


        else if (typeof(T) == typeof(UI_StageEndPanel))
        {
            stageEndPanel = (compoenent as UI_StageEndPanel);

        }


        if (typeof(T) == typeof(DamageMngPanel))
        {
            Debug.Log("********damage*******");
            Vector3 panelPos;
            if (_gameobject.tag == "Enemy")
            {
                enemy = _gameobject.GetComponent<Enemy>();
                panelPos = enemy.hitPos.transform.position;
            }
            else if (_gameobject.tag == "Turret")
            {
                turret = _gameobject.GetComponent<Turret>();
                panelPos = turret.hitPos.transform.position;
            }
            else
                return null;

            screenPos = Camera.main.WorldToScreenPoint(panelPos);
            go.transform.position = screenPos;
        }

        if (typeof(T) == typeof(KillRewardMngPanel))
        {            
            Vector3 panelPos;

            enemy = _gameobject.GetComponent<Enemy>();
            panelPos = enemy.hitPos.transform.position;

            screenPos = Camera.main.WorldToScreenPoint(panelPos);
            go.transform.position = screenPos;
        }


        return go;
    }


    public void DisablePanel<T>(GameObject go) where T: UnityEngine.Component
    {
        //예외처리
        if (go == null)
            return;

        T compoenent = go.GetComponent<T>();

        //예외처리
        if (compoenent == null)
            return;

        string filePath = null;

        if (typeof(T) == typeof(UI_TurretMngPanel))
        {
            filePath = (compoenent as UI_TurretMngPanel).filePath;
            turretMngPanel = null;
        }
        else if (typeof(T) == typeof(UI_TurretInfoPanel))
        {
            filePath = (compoenent as UI_TurretInfoPanel).filePath;
            turretInfoPanel = null;
        }
        else if (typeof(T) == typeof(StageMngPanel))
        {
            filePath = (compoenent as StageMngPanel).filePath;
            stageMngPanel = null;
        }
        else if (typeof(T) == typeof(StatusMngPanel))
        {
            filePath = (compoenent as StatusMngPanel).filePath;
            statusMngPanel = null;

        }
        else if (typeof(T) == typeof(DamageMngPanel))
        {
            filePath = (compoenent as DamageMngPanel).filePath;
            damagePanels.Remove(go);
            damageMngPanel = null;
        }
        else if (typeof(T) == typeof(UI_ResourcePanel))
        {
            filePath = (compoenent as UI_ResourcePanel).filePath;
            resoursePanel = null;
        }
        else if (typeof(T) == typeof(UI_ConstructionGauge))
        {
            filePath = (compoenent as UI_ConstructionGauge).filePath;
        }
        else if (typeof(T) == typeof(GoodsMngPanel))
        {
            filePath = (compoenent as GoodsMngPanel).filePath;
            goodsMngPanel = null;
        }

        else
            return;

        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, go);

        // 패널 HPBar 초기화
        
    }

}
