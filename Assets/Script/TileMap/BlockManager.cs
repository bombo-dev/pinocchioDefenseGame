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
    GameObject fieldGo;

    [SerializeField]
    GameObject[] field;

    [SerializeField]
    GameObject Light_RealTime_Normal;
    [SerializeField]
    GameObject Light_RealTime_Hard_Enemy;
    [SerializeField]
    GameObject Light_RealTime_Hard;

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
        fieldGo.SetActive(true);

        if (SystemManager.Instance.UserInfo.selectMode == 0) // - �븻
        {
            if (!Light_RealTime_Normal.activeSelf)
                Light_RealTime_Normal.SetActive(true);

            if (Light_RealTime_Hard.activeSelf)
                Light_RealTime_Hard.SetActive(false);

            if (Light_RealTime_Hard_Enemy.activeSelf)
                Light_RealTime_Hard_Enemy.SetActive(false);
        }
        else
        {
            if (Light_RealTime_Normal.activeSelf)
                Light_RealTime_Normal.SetActive(false);

            if (!Light_RealTime_Hard.activeSelf)
                Light_RealTime_Hard.SetActive(true);

            if (!Light_RealTime_Hard_Enemy.activeSelf)
                Light_RealTime_Hard_Enemy.SetActive(true);
        }
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
        UserInfo userInfo = SystemManager.Instance.UserInfo;

        if (userInfo.selectMode == 0)// -�븻
        {
            if (userInfo.selectedStageNum == 0)//Ʃ�丮��
                fieldGo = field[0];
            else if (userInfo.selectedStageNum <= 5)
                fieldGo = field[1];
            else if (userInfo.selectedStageNum <= 10)
                fieldGo = field[2];
            else if (userInfo.selectedStageNum <= 15)
                fieldGo = field[3];
            else if (userInfo.selectedStageNum <= 20)
                fieldGo = field[4];
            else if (userInfo.selectedStageNum <= 25)
                fieldGo = field[5];
            else if (userInfo.selectedStageNum <= 30)
                fieldGo = field[6];
            else if (userInfo.selectedStageNum <= 35)
                fieldGo = field[7];
            else if (userInfo.selectedStageNum <= 40)
                fieldGo = field[8];
        }
        else // - �ϵ�
        {
            if (userInfo.selectedStageNum_hard == 0)//Ʃ�丮��
                fieldGo = field[0];
            else if (userInfo.selectedStageNum_hard <= 5)
                fieldGo = field[1];
            else if (userInfo.selectedStageNum_hard <= 10)
                fieldGo = field[2];
            else if (userInfo.selectedStageNum_hard <= 15)
                fieldGo = field[3];
            else if (userInfo.selectedStageNum_hard <= 20)
                fieldGo = field[4];
            else if (userInfo.selectedStageNum_hard <= 25)
                fieldGo = field[5];
            else if (userInfo.selectedStageNum_hard <= 30)
                fieldGo = field[6];
            else if (userInfo.selectedStageNum_hard <= 35)
                fieldGo = field[7];
            else if (userInfo.selectedStageNum_hard <= 40)
                fieldGo = field[8];
        }

        //targetArr�ʱ�ȭ
        for (int i = 0; i < fieldGo.transform.GetChild(0).childCount; i++)
        {
            targetList.Add(fieldGo.transform.GetChild(0).GetChild(i).gameObject);
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
