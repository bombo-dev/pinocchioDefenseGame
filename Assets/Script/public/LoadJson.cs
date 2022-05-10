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
    /// Json 파일로부터 Json 데이터 가져오기
    /// </summary>
    public DefenseFlowDataList PrepareGameFlowJsonData()
    {
        DefenseFlowData[] defenseFlowDatas = new DefenseFlowData[3];

        //Json 불러오기
        string filePath;

        //filePath = Application.persistentDataPath + "/Test";
        filePath = Path.Combine(Application.streamingAssetsPath, "Test");

        filePath += ".Json";

        string JsonString = File.ReadAllText(filePath);

        DefenseFlowDataList datas = JsonUtility.FromJson<DefenseFlowDataList>(JsonString);

        return datas;
    }
}
