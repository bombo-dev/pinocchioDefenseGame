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
            turretMngPanel = (compoenent as UI_TurretMngPanel);
        else if (typeof(T) == typeof(UI_TurretInfoPanel))
        {
            turretInfoPanel = (compoenent as UI_TurretInfoPanel);
            (compoenent as UI_TurretInfoPanel).Reset();
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

        // HPBar ����Ʈ�� ����
        if (type.Name == "Turret")
            turretHPBars.Add(go);
        else if (type.Name == "Enemy")
            enemyHPBars.Add(go);

        T compoenent = go.GetComponent<T>();

        if (typeof(T) == typeof(StatusMngPanel))
        {
            statusMngPanel = (compoenent as StatusMngPanel);
            // (compoenent as StatusMngPanel).Reset();
            
        }
        else
            return;

        //�г� ��ġ �ʱ�ȭ
        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(startPos.x, startPos.y+30, startPos.z));
        go.transform.position = screenPos;
       
        return;

    }
    
    public void ReorganizationPanelList(int removePanelIndex)
    {
        List<GameObject> tempPanels = new List<GameObject>();
        int index = 0;

        for (int i = 0; i < turretHPBars.Count; i++)
        {
            //������ gameObject�� ����
            if (i != removePanelIndex)
            {
                //enemies[i]�� null�̸� ����
                if (turretHPBars[i])
                {
                    //����Ʈ �籸��
                    tempPanels.Add(turretHPBars[i]);
                    //panelIndex��ȣ �ʱ�ȭ
                    turretHPBars[i].GetComponent<StatusMngPanel>().HPBarsListIndex = index;

                    index++;
                }
            }
        }//end of for

        turretHPBars = tempPanels;
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
        else
            return;

        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, go);
    }

}
