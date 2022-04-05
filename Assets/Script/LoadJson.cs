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
    /// Json 파일로부터 Json 데이터 가져오기
    /// </summary>
    public DefenseFlowDataList PrepareGameFlowJsonData()
    {
        DefenseFlowData[] defenseFlowDatas = new DefenseFlowData[3];

        //Json 불러오기
        string filePath;
        filePath = Path.Combine(Application.streamingAssetsPath, "Test");

        filePath += ".Json";
        string JsonString = File.ReadAllText(filePath);

        DefenseFlowDataList datas = JsonUtility.FromJson<DefenseFlowDataList>(JsonString);

        Debug.Log(datas.datas[0].defenseFlowDataArr[0].enemyFlowIndexArr[3]);

        return datas;
    }
}
