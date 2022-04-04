using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadJson : MonoBehaviour
{
    /// <summary>
    /// Json 파일로부터 Json 데이터 가져오기
    /// </summary>
    public void PrepareGameFlowJsonData()
    {
        //Json 불러오기
        string filePath;
        filePath = Path.Combine(Application.streamingAssetsPath, "Json");

        /*
        //자료구조 초기화
        LocalizationData.Clear();

        filePath += ".Json";
        string JsonString = File.ReadAllText(filePath);

        LocalizationDataList datas = JsonUtility.FromJson<LocalizationDataList>(JsonString);
        for (int i = 0; i < datas.datas.Count; i++)
        {
            Debug.Log("/" + datas.datas[i].key + "/" + datas.datas[i].value);
            LocalizationData.Add(datas.datas[i].key, datas.datas[i].value);
        }
        */
    }
}
