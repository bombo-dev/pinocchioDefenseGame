using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public List<GameObject> rewardPanels;

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
    public RewardsMngPanel rewardsMngPanel;
    public UI_StageEndPanel stageEndPanel;
    public UI_OptionPopUpPanel optionPopUpPanel;
    public UI_TutorialPanel tutorialPanel;
    public UI_BookPanel bookPanel;
    public UI_BossPanel bossPanel;

    [SerializeField]
    Transform canvas;

    //filePath, cacheCount 저장
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;


    // Start is called before the first frame update
    void Start()
    {
        PrepareData();

        //게임씬 - 바로 생성해야할 패널 생성
        EnableFixedPanel(SceneManager.GetActiveScene().buildIndex);
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
    public void EnableFixedPanel(int sceneIndex)
    {
        switch (sceneIndex)
        {
            //로비씬
            case 0:
                //로비패널
                EnablePanel<UI_LobbyPanel>(0);

                //옵션 팝업 패널
                EnablePanel<UI_OptionPopUpPanel>(1);

                // Load 추가
                SaveLoad Load = new SaveLoad();
                Load.LoadUserInfo();


                EnablePanel<UI_BookPanel>(2);

                //타이틀 맨 앞으로
                if (SystemManager.Instance.firstRun)
                {
                    GameObject.FindObjectOfType<Title_Fade>().fade.gameObject.SetActive(true);

                    GameObject.FindObjectOfType<Title_Fade>().fade.transform.SetAsLastSibling();

                    SystemManager.Instance.firstRun = false;
                }
                else
                {
                    GameObject.FindObjectOfType<Title_Fade>().fade.gameObject.SetActive(false);
                }

                break;
            //로딩씬
            case 1:
                break;
            //게임씬
            case 2:
                if (SystemManager.Instance.GameFlowManager.gameState == GameFlowManager.GameState.Start)
                {
                    //자원표시 패널
                    EnablePanel<UI_ResourcePanel>(4);

                    //옵션 팝업 패널
                    EnablePanel<UI_OptionPopUpPanel>(11);

                    //튜토리얼 패널
                    if (SystemManager.Instance.UserInfo.selectMode == 0)//노말
                    {
                        if (SystemManager.Instance.UserInfo.selectedStageNum == 0)
                            EnablePanel<UI_TutorialPanel>(13);
                    }

                }
                else if (SystemManager.Instance.GameFlowManager.gameState == GameFlowManager.GameState.Defense)
                {
                    //게임옵션 패널
                    EnablePanel<UI_OptionPanel>(7);

                    //터렛 소환 패널
                    EnablePanel<UI_TurretMngPanel>(0);

                    //소환되어있는 터렛 정보 패널
                    EnablePanel<UI_TurretInfoPanel>(1);
                    if (turretInfoPanel)
                        turretInfoPanel.Reset();
                }     
                break;
            //게임씬
            case 3:
                //옵션 팝업 패널
                EnablePanel<UI_OptionPopUpPanel>(0);
                break;
        }
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

                statusMngPanel.randPos = randNum.Next(0, 10);
                Debug.Log("난수 생성=" + statusMngPanel.randPos);
                panelPos = new Vector3(enemy.hpPos.transform.position.x, enemy.hpPos.transform.position.y + statusMngPanel.randPos, enemy.hpPos.transform.position.z);

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
        else if (typeof(T) == typeof(RewardsMngPanel))
        {
            rewardsMngPanel = (compoenent as RewardsMngPanel);
        }
        else if (typeof(T) == typeof(UI_StageEndPanel))
        {
            stageEndPanel = (compoenent as UI_StageEndPanel);

        }
        else if (typeof(T) == typeof(UI_OptionPopUpPanel))
        {
            optionPopUpPanel = (compoenent as UI_OptionPopUpPanel);

        }
        else if (typeof(T) == typeof(DamageMngPanel))
        {
            damageMngPanel = (compoenent as DamageMngPanel);
            // hpBar 패널 위치 초기화
            damagePanels.Add(go);

            System.Random random = new System.Random();
            damageMngPanel.randPosX = random.Next(0, 10);
            damageMngPanel.randPosY = random.Next(0, 7);

            Debug.Log("randPosX=" + damageMngPanel.randPosX);
            Debug.Log("randPosY=" + damageMngPanel.randPosY);

            Vector3 panelPos;
            if (_gameobject.tag == "Enemy")
            {
                enemy = _gameobject.GetComponent<Enemy>();
                panelPos = new Vector3(enemy.hitPos.transform.position.x + damageMngPanel.randPosX,
                                                      enemy.hitPos.transform.position.y + damageMngPanel.randPosY,
                                                      enemy.hitPos.transform.position.z); ;
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

        else if (typeof(T) == typeof(KillRewardMngPanel))
        {
            // 활성화된 패널을 리스트에 저장
            rewardPanels.Add(go);
            Vector3 panelPos;

            enemy = _gameobject.GetComponent<Enemy>();
            panelPos = enemy.hitPos.transform.position;

            screenPos = Camera.main.WorldToScreenPoint(panelPos);
            go.transform.position = screenPos;


        }
        else if (typeof(T) == typeof(UI_TutorialPanel))
        {
            tutorialPanel = (compoenent as UI_TutorialPanel);
        }
        else if (typeof(T) == typeof(UI_BookPanel))
        {
            bookPanel = (compoenent as UI_BookPanel);
        }
        else if (typeof(T) == typeof(UI_BossPanel))
        {
            bossPanel = (compoenent as UI_BossPanel);
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
        else if (typeof(T) == typeof(RewardsMngPanel))
        {
            filePath = (compoenent as RewardsMngPanel).filePath;
            rewardsMngPanel = null;
        }
        else if (typeof(T) == typeof(KillRewardMngPanel))
        {
            rewardPanels.Remove(go);
            filePath = (compoenent as KillRewardMngPanel).filePath;
        }

        else
            return;

        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, go);

        // 패널 HPBar 초기화
        
    }

}
