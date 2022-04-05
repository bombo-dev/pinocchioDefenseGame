using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadJson : MonoBehaviour
{
    private void Start()
    {
        PrepareGameFlowJsonData();
    }
    /// <summary>
    /// Json ���Ϸκ��� Json ������ ��������
    /// </summary>
    public void PrepareGameFlowJsonData()
    {
        DefenseFlowData[] defenseFlowDatas = new DefenseFlowData[3];

        //List<DefenseFlowDataArr> defenseFlowDataList = new List<DefenseFlowDataArr>();

        //Json �ҷ�����
        string filePath;
        filePath = Path.Combine(Application.streamingAssetsPath, "Test");

        filePath += ".Json";
        string JsonString = File.ReadAllText(filePath);

        DefenseFlowDataList datas = JsonUtility.FromJson<DefenseFlowDataList>(JsonString);

        Debug.Log(datas.datas[0].defenseFlowDataArr[2].targetTileIndexArr[7]);
        for (int i = 0; i < datas.datas.Count; i++)
        {
            Debug.Log(datas.datas[i]);
        }
        
        
    }
}
