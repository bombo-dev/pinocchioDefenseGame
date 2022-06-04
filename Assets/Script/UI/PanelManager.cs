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

    // Ȱ��ȭ�� turret�� HPBar �г��� ������ ����Ʈ
    public List<GameObject> turretHPBars;

    // Ȱ��ȭ�� enemy�� HPBar �г��� ������ ����Ʈ
    public List<GameObject> enemyHPBars;



    [Header("PanelCachesInfo")]
    //Load�� Panel ������ ����
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    // Ȱ��ȭ�� panel�� �޾ƿ� ����Ʈ
    public UI_TurretMngPanel turretMngPanel;
    public UI_TurretInfoPanel turretInfoPanel;
    public StageMngPanel stageMngPanel;
    public StatusMngPanel statusMngPanel;
    public DamageMngPanel damageMngPanel;


    [SerializeField]
    Transform canvas;

    //filePath, cacheCount ����
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;


    // Start is called before the first frame update
    void Start()
    {
        PrepareData();
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
    ///  Panel ��ü�� ����
    /// </summary>
    /// <typeparam name="T">�г��� ������ �ִ� UI_Panel ��ũ��Ʈ</typeparam>
    /// <param name="panelIndex">������ �г� ��ȣ</param>
    public void EnablePanel<T>(int panelIndex) where T : UnityEngine.Component
    {
        //����ó��
        if (panelIndex >= prefabCacheDatas.Length || prefabCacheDatas[panelIndex].filePath == null)
            return;

        //������ ������ ���ӿ�����Ʈ ���� �޾ƿ���
        GameObject go = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[panelIndex].filePath);

        if (go == null)
            return;

        
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
        /*
        else if (typeof(T) == typeof(StatusMngPanel))
        {
            statusMngPanel = (compoenent as StatusMngPanel);
            // (compoenent as StatusMngPanel).Reset();
            
        }
        */
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelIndex"></param>
    /// <param name="startPos"></param>
    /// <param name="hpBarsPanelIndex"></param>
    public void EnablePanel<T>(int panelIndex, Vector3 startPos, int hpBarsPanelIndex, System.Type type) where T : UnityEngine.Component
    {
        
        //����ó��
        if (panelIndex >= prefabCacheDatas.Length || prefabCacheDatas[panelIndex].filePath == null)
            return;


        //������ ������ ���ӿ�����Ʈ ���� �޾ƿ���
        GameObject go = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[panelIndex].filePath);

        if (go == null)
            return;

        T compoenent = go.GetComponent<T>();

        if (typeof(T) == typeof(StatusMngPanel))
        {
            statusMngPanel = (compoenent as StatusMngPanel);
            //(compoenent as StatusMngPanel).Reset();
            //go.transform.SetAsFirstSibling();
        }
        else if (typeof(T) == typeof(DamageMngPanel))
        {
            damageMngPanel = (compoenent as DamageMngPanel);
            //go.transform.SetAsFirstSibling();
        }
        else
            return;

        if (typeof(T) == typeof(StatusMngPanel))
        {
            // HPBar ����Ʈ�� ����
            if (type.Name == "Turret")
            {
                turretHPBars.Add(go);
                //statusMngPanel.turretHPBarIndex = turretHPBars.FindIndex(x => x == go);
                statusMngPanel.SetHPBarColor();
            }

            else if (type.Name == "Enemy")
            {
                enemyHPBars.Add(go);
                //statusMngPanel.enemyHPBarIndex = enemyHPBars.FindIndex(x => x == go);
            }
        }        

        //�г� ��ġ �ʱ�ȭ
        if (typeof(T) == typeof(StatusMngPanel))
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(startPos.x, startPos.y + 30, startPos.z));
            go.transform.position = screenPos;
        }

        else if (typeof(T) == typeof(DamageMngPanel))
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(startPos.x, startPos.y, startPos.z));
            go.transform.position = screenPos;
        }      

    }
    
    public void ReorganizationPanelList(int removePanelIndex, System.Type type)
    {
        List<GameObject> tempPanels = new List<GameObject>();
        int index = 0;
        int listLength;        

        if (type.Name == "Turret")
            listLength = turretHPBars.Count;
        else if (type.Name == "Enemy")
            listLength = enemyHPBars.Count;
        else
        {
            Debug.LogError("ReorganizationPanelList Error!");
            return;
        }

        for (int i = 0; i < listLength; i++)
        {
            //������ gameObject�� ����
            if (i != removePanelIndex)
            {
                //enemies[i]�� null�̸� ����
                if (type.Name == "Turret" && turretHPBars[i])
                {
                    //����Ʈ �籸��
                    tempPanels.Add(turretHPBars[i]);
                    //panelIndex��ȣ �ʱ�ȭ
                    turretHPBars[i].GetComponent<StatusMngPanel>().turretHPBarIndex = index;

                    index++;
                }
                else if (type.Name == "Enemy" && enemyHPBars[i])
                {
                    //����Ʈ �籸��
                    tempPanels.Add(enemyHPBars[i]);
                    //panelIndex��ȣ �ʱ�ȭ
                    enemyHPBars[i].GetComponent<StatusMngPanel>().enemyHPBarIndex = index;

                    index++;
                }
            }
        }//end of for

        if (type.Name == "Turret")
            turretHPBars = tempPanels;
        else if (type.Name == "Enemy")
            enemyHPBars = tempPanels;
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
            damageMngPanel = null;
        }
        else
            return;

        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, go);

        // �г� HPBar �ʱ�ȭ
        
    }

}
