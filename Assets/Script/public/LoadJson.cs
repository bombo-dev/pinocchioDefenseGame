using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoadJson : MonoBehaviour
{
    private void Start()
    {
        
        // *************** 암호화된 Json 파일 실행하는 부분 *****************************
        //Save(PathInit()); // Test.json 복호화 후 저장
        //PrepareGameFlowJsonData(); // 게이트 json 동기화
        // ********************************************************************************


        // **************** 암호화 되어 있지 않은 Json 파일 실행하는 부분 ************************
        // PrepareGameFlowDecryptJsonData();
        // ********************************************************************************
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
            string load = Load(filePath);

            return JsonToObject<DefenseFlowDataList>(load);
        }
        // Windows PC 플레이어 에서 실행
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Debug.Log("PC 플레이어에서 실행");


            // string jsonString = File.ReadAllText(filePath);
            // 암호화되어있는 json파일 불러오기
            string load = Load(filePath);

            return JsonToObject<DefenseFlowDataList>(load);
        }

        // MAC OS Editor
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            Debug.Log("OS Editor Execute");
            string load = Load(filePath);

            return JsonToObject<DefenseFlowDataList>(load);
        }

        // MAC OS Player
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("OS Player에서 실행");
            string load = Load(filePath);

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
            string load = Load(realPath);
            return JsonToObject<DefenseFlowDataList>(load);
        }
    }

    // 암호화 되어 있지 않은 Test.json 가져오는 메서드
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
            string load = Load(filePath);

            return JsonToObject<DefenseFlowDataList>(load);
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
    
    // JsonData를 읽기만 하는 메서드
    public string ReadJsonFileToString(string filePath)
    {
        string jsonString;

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            jsonString = File.ReadAllText(filePath);

            return jsonString;
        }

        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            jsonString = File.ReadAllText(filePath);

            return jsonString;
        }

        else
        {
            string originPath = filePath;
            #pragma warning disable 612, 618
            WWW reader = new WWW(originPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + ".Json";
            File.WriteAllBytes(realPath, reader.bytes);

            jsonString = File.ReadAllText(realPath);
            return jsonString;
        }
    }

    public void Save(string filePath)
    {
        // Json 복호화
        JsonToDecrypt(filePath);
        string save = ReadJsonFileToString(filePath);
        save = EncryptDecrypt.Encrypt(save, "key");
        // 암호화된 Json 저장
        File.WriteAllText(filePath, save);
    }

    public string Load(string filePath)
    {
        string load = File.ReadAllText(filePath);
        load = EncryptDecrypt.Decrypt(load, "key");
        return load;
    }

    public void JsonToDecrypt(string filePath)
    {
        string load = File.ReadAllText(filePath);
        load = EncryptDecrypt.Decrypt(load, "key");
        File.WriteAllText(filePath, load);
    }



    /// <summary>
    /// 암호화 복호화 관련 메서드
    /// </summary>
    /// <returns></returns>


    // 복호화
    public static string Decrypt(string textToDecrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;

        rijndaelCipher.Padding = PaddingMode.PKCS7;

        rijndaelCipher.KeySize = 128;

        rijndaelCipher.BlockSize = 128;

        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);

        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);

        byte[] keyBytes = new byte[16];

        int len = pwdBytes.Length;

        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }

        Array.Copy(pwdBytes, keyBytes, len);

        rijndaelCipher.Key = keyBytes;

        rijndaelCipher.IV = keyBytes;

        byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);

        return Encoding.UTF8.GetString(plainText);
    }

    // 암호화
    public static string Encrypt(string textToEncrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;

        rijndaelCipher.Padding = PaddingMode.PKCS7;

        rijndaelCipher.KeySize = 128;

        rijndaelCipher.BlockSize = 128;

        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);

        byte[] keyBytes = new byte[16];

        int len = pwdBytes.Length;

        if (len > keyBytes.Length)

        {
            len = keyBytes.Length;
        }

        Array.Copy(pwdBytes, keyBytes, len);

        rijndaelCipher.Key = keyBytes;

        rijndaelCipher.IV = keyBytes;

        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();

        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);

        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
    }

}
