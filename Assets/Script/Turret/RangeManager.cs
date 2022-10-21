using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeManager : MonoBehaviour
{
    //Load�� Range ������ ����
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    [SerializeField]
    public Transform rangeParents;

    //filePath, cacheCount ����
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;

    //������ ���ӿ�����Ʈ 
    GameObject currentRange;

    // Start is called before the first frame update
    void Start()
    {
        PrepareData();
    }

    /// <summary>
    /// �� �ε� �� Range ĳ�� �����͸� �������� ������ �Լ� ȣ�� : ������
    /// </summary>
    void PrepareData()
    {
        for (int i = 0; i < prefabCacheDatas.Length; i++)
        {
            SystemManager.Instance.PrefabCacheSystem.GeneratePrefabCache(prefabCacheDatas[i].filePath, prefabCacheDatas[i].cacheCount, Load(prefabCacheDatas[i].filePath), rangeParents);
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
    ///  ��Ÿ� ǥ�� ������Ʈ ���� : ������
    /// </summary>
    /// <param name="rangeIndex">��Ÿ� ǥ�� ������Ʈ �ε���</param>
    /// <param name="range">ǥ���� ��Ÿ�</param>
    /// <param name="rangePos">��Ÿ� ��ġ</param>
    public void EnableRange(int rangeIndex, int range, Vector3 rangePos)
    {

        //����ó��
        if (rangeIndex >= prefabCacheDatas.Length || prefabCacheDatas[rangeIndex].filePath == null)
            return;

        //���� �Ǿ��ִ� ������Ʈ ������ �����
        DisableRange(rangeIndex);

        //������ ������ ���ӿ�����Ʈ ���� �޾ƿ���
        currentRange = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[rangeIndex].filePath);

        if (currentRange == null)
            return;

        //��Ÿ� ǥ�� ������Ʈ ��ġ ����
        currentRange.transform.position = rangePos;

        //��Ÿ� ǥ�� ������Ʈ ũ�� ����
        float rangeScale = Mathf.Sqrt(range) * 1.4625f;
        currentRange.transform.localScale = new Vector3(rangeScale, rangeScale, currentRange.transform.localScale.z);

    }

    /// <summary>
    /// ��Ÿ� �����
    /// </summary>
    /// <param name="rangeIndex">���� ��Ÿ� ������Ʈ �ε���</param>
    public void DisableRange(int rangeIndex)
    {
        if (currentRange)
            SystemManager.Instance.PrefabCacheSystem.DisablePrefabCache(prefabCacheDatas[rangeIndex].filePath, currentRange);
    }
}
