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
        //filePath = Path.Combine(Application.streamingAssetsPath, "Test");
        //filePath += ".Json";
        filePath = PathCheck();

        //string JsonString = File.ReadAllText(filePath);
        // DefenseFlowDataList datas = JsonToObject(jsonString);

        return LoadJsonFile<DefenseFlowDataList>(filePath);
    }

    //Json File PC, 모바일 경로 체크
    public string PathCheck()
    {
        string filePath;

        // PC 경로 체크
        if (File.Exists(Path.Combine(Application.streamingAssetsPath, "Test"))) {
            filePath = Path.Combine(Application.streamingAssetsPath, "Test");
            filePath += ".Json";
            return filePath;
            }

        // 모바일 경로 체크
        else {
            filePath = Application.persistentDataPath + "/Test";
            filePath += ".Json";
            return filePath;
        }
    }

    // JsonData의 객체화 메소드
    public static DefenseFlowDataList JsonToObject<DefenseFlowDataList>(string jsonString) 
    {
        return JsonUtility.FromJson<DefenseFlowDataList>(jsonString);
    }

    // JsonData 불러오는 메소드
    public static DefenseFlowDataList LoadJsonFile<DefenseFlowDataList>(string filePath) {
        string jsonString =  File.ReadAllText(filePath);
        return JsonToObject<DefenseFlowDataList>(jsonString);
    }

}
