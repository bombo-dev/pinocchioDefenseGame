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
        //filePath = Path.Combine(Application.streamingAssetsPath, "Test");
        //filePath += ".Json";
        filePath = PathCheck();

        //string JsonString = File.ReadAllText(filePath);
        // DefenseFlowDataList datas = JsonToObject(jsonString);

        return LoadJsonFile<DefenseFlowDataList>(filePath);
    }

    //Json File PC, ����� ��� üũ
    public string PathCheck()
    {
        string filePath;

        // PC ��� üũ
        if (File.Exists(Path.Combine(Application.streamingAssetsPath, "Test"))) {
            filePath = Path.Combine(Application.streamingAssetsPath, "Test");
            filePath += ".Json";
            return filePath;
            }

        // ����� ��� üũ
        else {
            filePath = Application.persistentDataPath + "/Test";
            filePath += ".Json";
            return filePath;
        }
    }

    // JsonData�� ��üȭ �޼ҵ�
    public static DefenseFlowDataList JsonToObject<DefenseFlowDataList>(string jsonString) 
    {
        return JsonUtility.FromJson<DefenseFlowDataList>(jsonString);
    }

    // JsonData �ҷ����� �޼ҵ�
    public static DefenseFlowDataList LoadJsonFile<DefenseFlowDataList>(string filePath) {
        string jsonString =  File.ReadAllText(filePath);
        return JsonToObject<DefenseFlowDataList>(jsonString);
    }

}
