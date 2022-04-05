using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadJson : MonoBehaviour
{
    /// <summary>
    /// Json ���Ϸκ��� Json ������ ��������
    /// </summary>
    public DefenseFlowDataList PrepareGameFlowJsonData()
    {
        DefenseFlowData[] defenseFlowDatas = new DefenseFlowData[3];

        //List<DefenseFlowDataArr> defenseFlowDataList = new List<DefenseFlowDataArr>();

        //Json �ҷ�����
        string filePath;
        filePath = Path.Combine(Application.streamingAssetsPath, "Test");

        filePath += ".Json";
        string JsonString = File.ReadAllText(filePath);

        DefenseFlowDataList datas = JsonUtility.FromJson<DefenseFlowDataList>(JsonString);

        return datas;
    }
}
