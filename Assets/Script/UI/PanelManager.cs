using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PanelManager : MonoBehaviour
{
    [Header("PanelCachesInfo")]
    //Load�� Panel ������ ����
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    // Ȱ��ȭ�� panel�� �޾ƿ� ����Ʈ
    public UI_TurretMngPanel turretMngPanel;
    public UI_TurretInfoPanel turretInfoPanel;

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
        else
            return;

        SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(filePath, go);
    }
}
