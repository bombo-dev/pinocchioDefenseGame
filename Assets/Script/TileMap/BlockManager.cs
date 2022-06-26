using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> targetList;

    //Load�� Block ������ ����
    Dictionary<string, GameObject> prefabCaChes = new Dictionary<string, GameObject>();

    // Ȱ��ȭ�� block�� �޾ƿ� ����Ʈ
    public List<GameObject> blocks;

    [SerializeField]
    Transform blockParents;

    //filePath, cacheCount ����
    [SerializeField]
    PrefabCacheData[] prefabCacheDatas;

    [SerializeField]
    GameObject testGo;

    [SerializeField]
    GameObject[] field;

    public GameObject[] tutorialNest;

    // Start is called before the first frame update
    void Start()
    {
        PrepareData();

        EnableField();
    }

    /// <summary>
    /// ���������� �´� �� Ȱ��ȭ : ������
    /// </summary>
    void EnableField()
    {
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        if (userInfo.selectedStageNum == 0)//Ʃ�丮��
            field[0].SetActive(true);
        else if (userInfo.selectedStageNum <= 5)
            field[1].SetActive(true);
        else if (userInfo.selectedStageNum <= 10)
            field[2].SetActive(true);
        else if (userInfo.selectedStageNum <= 15)
            field[3].SetActive(true);
        else if (userInfo.selectedStageNum <= 20)
            field[4].SetActive(true);
    }

    /// <summary>
    /// �� �ε� �� Block ĳ�� �����͸� �������� ������ �Լ� ȣ�� : ������
    /// </summary>
    void PrepareData()
    {
        for (int i = 0; i < prefabCacheDatas.Length; i++)
        {
            SystemManager.Instance.PrefabCacheSystem.GeneratePrefabCache(prefabCacheDatas[i].filePath, prefabCacheDatas[i].cacheCount, Load(prefabCacheDatas[i].filePath), blockParents);
        }

        //���������� �´� �� ����
        //EnableBlock(SystemManager.Instance.GameFlowManager.block);

        //targetArr�ʱ�ȭ
        for (int i = 0; i < testGo.transform.GetChild(0).childCount; i++)
        {
            targetList.Add(testGo.transform.GetChild(0).GetChild(i).gameObject);
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
    /// Block ��ü�� ����
    /// </summary>
    /// <param name="enemyIndex">������ Block�� ����� �ε���</param>
    public void EnableBlock(int blockIndex)
    {
        //����ó��
        if (blockIndex >= prefabCacheDatas.Length || prefabCacheDatas[blockIndex].filePath == null)
            return;

        //������ ������ ���ӿ�����Ʈ ���� �޾ƿ���
        GameObject go = SystemManager.Instance.PrefabCacheSystem.EnablePrefabCache(prefabCacheDatas[blockIndex].filePath);

        if (go == null)
            return;

        blocks.Add(go);

        //targetArr�ʱ�ȭ
        for (int i = 0; i < go.transform.GetChild(0).childCount; i++)
        {
            targetList.Add(go.transform.GetChild(0).GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// ������ �ε��� �迭�� ������ �ε����� �´� ���ӿ�����Ʈ �迭�� �����Ͽ� ��ȯ
    /// </summary>
    /// <param name="targetIndexArr">Ÿ�ϸ� ��ȣ �ε����� �̷���� �迭</param>
    /// <returns>Ÿ�ϸ� ���� ������Ʈ�� �̷���� �迭</returns>
    public GameObject[] CreateTargetArr(int[] targetIndexArr)
    {
        //����ó��
        if (targetIndexArr.Length == 0)
            return null;

        GameObject[] goArr = new GameObject[targetIndexArr.Length];

        //Ÿ�� �ε��� �迭�� Ÿ�� ���ӿ�����Ʈ �迭���� ���
        for (int i = 0; i < targetIndexArr.Length; i++)
        {
            goArr[i] = targetList[targetIndexArr[i]];
        }

        return goArr;
    }
}
