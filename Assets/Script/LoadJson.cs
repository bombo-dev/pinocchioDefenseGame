using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class LoadJson : MonoBehaviour
{
    [SerializeField]
    Text Test;
    [SerializeField]
    Text Test2;
    private void Start()
    {
        PrepareGameFlowJsonData();
    }
    /// <summary>
    /// Json ���Ϸκ��� Json ������ ��������
    /// </summary>
    public DefenseFlowDataList PrepareGameFlowJsonData()
    {
        DefenseFlowData[] defenseFlowDatas = new DefenseFlowData[3];

        //Json �ҷ�����
        string filePath;

        //filePath = Application.persistentDataPath + "/Test";
        filePath = Path.Combine(Application.streamingAssetsPath, "Test");

        filePath += ".Json";

        string JsonString = File.ReadAllText(filePath);

        DefenseFlowDataList datas = JsonUtility.FromJson<DefenseFlowDataList>(JsonString);

        return datas;
    }
}
