using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadJson : MonoBehaviour
{
    /// <summary>
    /// Json ���Ϸκ��� Json ������ ��������
    /// </summary>
    public void PrepareGameFlowJsonData()
    {
        //Json �ҷ�����
        string filePath;
        filePath = Path.Combine(Application.streamingAssetsPath, "Json");

        /*
        //�ڷᱸ�� �ʱ�ȭ
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
