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

    // Ȱ��ȭ�� damage �г��� ������ ����Ʈ
    public List<GameObject> damagePanels;



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
    public GoodsMngPanel goodsMngPanel;
    public UI_StageEndPanel stageEndPanel;


    [SerializeField]
    Transform canvas;

    //filePath, cacheCount ����
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;
 


    // Start is called before the first frame update
    void Start()
    {
        PrepareData();

        //�ٷ� �����ؾ��� �г� ����
        EnableFixedPanel();
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
        else if (typeof(T) == typeof(DamageMngPanel))
        {
            damageMngPanel = (compoenent as DamageMngPanel);
            // hpBar �г� ��ġ �ʱ�ȭ   
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
                Debug.Log("���� ����=" + randNum.Next(0, 10));
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
        else if (typeof(T) == typeof(GoodsMngPanel))
        {
            filePath = (compoenent as GoodsMngPanel).filePath;
            goodsMngPanel = null;
        }

        else
            return;

        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, go);

        // �г� HPBar �ʱ�ȭ
        
    }

}
