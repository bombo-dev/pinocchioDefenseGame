using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GateJson : MonoBehaviour
{
    private void Start()
    {
        /**
         *  GameFlowManager.cs 에서 실행
         */
    }

    /// <summary>
    /// Json 파일을 동기화 하는 메서드
    /// </summary>
    public DefenseFlowDataList PrepareGameFlowJsonData()
    {
        DefenseFlowData[] defenseFlowDatas = new DefenseFlowData[3];

        //Json 경로 초기화
        string filePath = Path.Combine(Application.streamingAssetsPath, "Spawn.Json");

        return LoadJsonFile<DefenseFlowDataList>(filePath);
    }


    /// <summary>
	/// 암호화 되어 있지 않은 Json파일 동기화하는 메서드
	/// </summary>
	
    public DefenseFlowDataList PrepareGameFlowDecryptJsonData()
    {
        DefenseFlowData[] defenseFlowDatas = new DefenseFlowData[3];

        //Json 경로 초기화
        string filePath = Path.Combine(Application.streamingAssetsPath, "Spawn.Json");

        return DecryptLoadJsonFile<DefenseFlowDataList>(filePath);

    }

    //Json filePath 초기화
    public string PathInit()
    {
            string filePath;
            filePath = Path.Combine(Application.streamingAssetsPath, "Spawn.Json");
            return filePath;
    }

    // JsonData를 객체화
    public DefenseFlowDataList JsonToObject<DefenseFlowDataList>(string jsonString) 
    {
        return JsonUtility.FromJson<DefenseFlowDataList>(jsonString);
    }

    // JsonData streamingAssets폴더에 저장되어 있는 json 파일 가져오기
    public DefenseFlowDataList LoadJsonFile<DefenseFlowDataList>(string filePath) {

        // 윈도우 유니티 편집기에서 실행
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("윈도우 유니티 편집기에서 실행");

            //string jsonString = File.ReadAllText(filePath);

            // 암호화되어있는 json파일 가져오기
            string load = GateJsonLoad(filePath);

            return JsonToObject<DefenseFlowDataList>(load);
        }
        // Windows PC 플레이어 에서 실행
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Debug.Log("PC 플레이어에서 실행");


            // string jsonString = File.ReadAllText(filePath);
            // 암호화되어있는 json파일 불러오기
            string load = GateJsonLoad(filePath);

            return JsonToObject<DefenseFlowDataList>(load);
        }

        // MAC OS Editor
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            Debug.Log("OS Editor Execute");
            string load = GateJsonLoad(filePath);

            return JsonToObject<DefenseFlowDataList>(load);
        }

        // MAC OS Player
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("OS Player에서 실행");
            string load = GateJsonLoad(filePath);

            return JsonToObject<DefenseFlowDataList>(load);
        }

        // 안드로이드 모바일 에서 실행
        else
        {
            string originPath = filePath;
            #pragma warning disable 612, 618
            WWW reader = new WWW(originPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + ".Json";
            File.WriteAllBytes(realPath, reader.bytes);


            //string jsonString = File.ReadAllText(realPath);
            //암호화 되어 있는 json파일 가져오기
            string load = GateJsonLoad(realPath);
            return JsonToObject<DefenseFlowDataList>(load);
        }
    }

    // 암호화 되어 있지 않은 Gate.json 가져오는 메서드
    public DefenseFlowDataList DecryptLoadJsonFile<DefenseFlowDataList>(string filePath)
    {

        // 윈도우 유니티 편집기에서 실행
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("윈도우 유니티 편집기에서 실행");

            string jsonString = File.ReadAllText(filePath);

            return JsonToObject<DefenseFlowDataList>(jsonString);
        }
        // Windows PC 플레이어 에서 실행
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Debug.Log("PC 플레이어에서 실행");

            string jsonString = File.ReadAllText(filePath);
            
            return JsonToObject<DefenseFlowDataList>(jsonString);
        }

        // MAC OS Editor 에서 실행
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            Debug.Log("Mac OS Editor에서 실행");
            string jsonString = File.ReadAllText(filePath);

            return JsonToObject<DefenseFlowDataList>(jsonString);
        }

        // MAC OS Player
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("OS Player Execute");
            string jsonString = File.ReadAllText(filePath);

            return JsonToObject<DefenseFlowDataList>(jsonString);
        }

        // // 안드로이드 모바일 에서 실행
        else
        {
            string originPath = filePath;
            
            #pragma warning disable 612, 618
            WWW reader = new WWW(originPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + ".Json";
            File.WriteAllBytes(realPath, reader.bytes);

            string jsonString = File.ReadAllText(realPath);
            
            return JsonToObject<DefenseFlowDataList>(jsonString);
        }
    }

    public void GateJsonSave(string filePath)
    {
        // Json 복호화
        EncryptDecrypt.Decrypt(filePath, "key");
        string save = File.ReadAllText(filePath);
        save = EncryptDecrypt.Encrypt(save, "key");
        // 암호화된 Json 저장
        File.WriteAllText(filePath, save);
    }

    public string GateJsonLoad(string filePath)
    {
        string load = File.ReadAllText(filePath);
        load = EncryptDecrypt.Decrypt(load, "chungwoonPinocchio");
        return load;
    }

    public void JsonToDecrypt(string filePath)
    {
        string load = File.ReadAllText(filePath);
        load = EncryptDecrypt.Decrypt(load, "key");
        File.WriteAllText(filePath, load);
    } 
}
