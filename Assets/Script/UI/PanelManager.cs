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

    // Ȱ��ȭ�� damage �г��� ������ ����Ʈ
    public List<GameObject> damagePanels;

    public List<GameObject> rewardPanels;

    [Header("PanelCachesInfo")]
    //Load�� Panel ������ ����
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    // Ȱ��ȭ�� panel�� �޾ƿ� ����Ʈ
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

    //filePath, cacheCount ����
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;


    // Start is called before the first frame update
    void Start()
    {
        PrepareData();

        //���Ӿ� - �ٷ� �����ؾ��� �г� ����
        EnableFixedPanel(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// �� �ε� �� Enemy ĳ�� �����͸� �������� ������ �Լ� ȣ�� : ������
    /// </summary>
    void PrepareData()
    {
        for (int i = 0; i < prefabCacheDatas.Length; i++)
        {
            SystemManager.Instance.PrefabCacheSystem.GeneratePrefabCache(prefabCacheDatas[i].filePath, prefabCacheDatas[i].cacheCount, Load(prefabCacheDatas[i].filePath), canvas);
        }
    }

    /// <summary>
    /// ���� ���� ���� �ٷ� ��Ÿ�� �ִ� ������ �г� ���� : ������
    /// </summary>
    public void EnableFixedPanel(int sceneIndex)
    {
        switch (sceneIndex)
        {
            //�κ��
            case 0:
                //�κ��г�
                EnablePanel<UI_LobbyPanel>(0);

                //�ɼ� �˾� �г�
                EnablePanel<UI_OptionPopUpPanel>(1);

                // Load �߰�
                SaveLoad Load = new SaveLoad();
                Load.LoadUserInfo();


                EnablePanel<UI_BookPanel>(2);

                //Ÿ��Ʋ �� ������
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
            //�ε���
            case 1:
                break;
            //���Ӿ�
            case 2:
                if (SystemManager.Instance.GameFlowManager.gameState == GameFlowManager.GameState.Start)
                {
                    //�ڿ�ǥ�� �г�
                    EnablePanel<UI_ResourcePanel>(4);

                    //�ɼ� �˾� �г�
                    EnablePanel<UI_OptionPopUpPanel>(11);

                    //Ʃ�丮�� �г�
                    if (SystemManager.Instance.UserInfo.selectMode == 0)//�븻
                    {
                        if (SystemManager.Instance.UserInfo.selectedStageNum == 0)
                            EnablePanel<UI_TutorialPanel>(13);
                    }

                }
                else if (SystemManager.Instance.GameFlowManager.gameState == GameFlowManager.GameState.Defense)
                {
                    //���ӿɼ� �г�
                    EnablePanel<UI_OptionPanel>(7);

                    //�ͷ� ��ȯ �г�
                    EnablePanel<UI_TurretMngPanel>(0);

                    //��ȯ�Ǿ��ִ� �ͷ� ���� �г�
                    EnablePanel<UI_TurretInfoPanel>(1);
                    if (turretInfoPanel)
                        turretInfoPanel.Reset();
                }     
                break;
            //���Ӿ�
            case 3:
                //�ɼ� �˾� �г�
                EnablePanel<UI_OptionPopUpPanel>(0);
                break;
        }
    }

    /// <summary>
    /// ������ ��θ� ���� ���ӿ�����Ʈ�� �����´� : ������
    /// </summary>
    /// <param name="filePath">�������� ������ִ� ���</param>
    /// <returns>��ο��� ������ ���� ������Ʈ</returns>
    GameObject Load(string filePath)
    {
        //�̹� ĳ�ÿ� ���ԵǾ� ���� ���
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
    ///  Panel ��ü�� ���� : ������
    /// </summary>
    /// <typeparam name="T">�г��� ������ �ִ� UI_Panel ��ũ��Ʈ</typeparam>
    /// <param name="panelIndex">������ �г� ��ȣ</param>
    /// /// <param name="_gameobject">�гο� ���ӿ�����Ʈ ������ �����ؾ� �� ��� ���</param>
    public GameObject EnablePanel<T>(int panelIndex, GameObject _gameobject = null) where T : UnityEngine.Component
    {
        
        //����ó��
        if (panelIndex >= prefabCacheDatas.Length || prefabCacheDatas[panelIndex].filePath == null)
            return null;
       
        //������ ������ ���ӿ�����Ʈ ���� �޾ƿ���
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

            //����ó��
            if (!_gameobject)
                return null;

            if (constructionGaguePanel)
            {
                //�г��� ���ӿ�����Ʈ ���� ����
                constructionGaguePanel.constructionTurret = _gameobject.GetComponent<ConstructionTurret>();

                //�Ǽ� ������ �г� ��ġ ����
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

                // hpBar�� ���ļ� �Ⱥ��̴� ������ �ذ��ϱ� ���� y�� ��ġ�� ���� ���� �����ֱ�
                System.Random randNum = new System.Random();

                statusMngPanel.randPos = randNum.Next(0, 10);
                Debug.Log("���� ����=" + statusMngPanel.randPos);
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
            // hpBar �г� ��ġ �ʱ�ȭ
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
            // Ȱ��ȭ�� �г��� ����Ʈ�� ����
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
        //����ó��
        if (go == null)
            return;

        T compoenent = go.GetComponent<T>();

        //����ó��
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

        // �г� HPBar �ʱ�ȭ
        
    }

}
