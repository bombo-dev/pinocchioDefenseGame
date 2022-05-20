using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine.UI;

public class LoadJson : MonoBehaviour
{
    private void Start()
    {
        // DeleteJson()
        // LoadFromJson()
        Save(PathInit());
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

        //Json 경로 체크
        filePath = PathInit();

        return LoadJsonFile<DefenseFlowDataList>(filePath);
    }

    //Json File 초기화
    public string PathInit()
    {
            string filePath;
            filePath = Path.Combine(Application.streamingAssetsPath, "Test.Json");
            return filePath;
    }

    // JsonData의 객체화 메소드
    public static DefenseFlowDataList JsonToObject<DefenseFlowDataList>(string jsonString) 
    {
        return JsonUtility.FromJson<DefenseFlowDataList>(jsonString);
    }

    // JsonData 불러오는 메소드
    public static DefenseFlowDataList LoadJsonFile<DefenseFlowDataList>(string filePath) {

        // 윈도우 유니티 에디터 경로 체크
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("유니티 에디터에서 실행");

            // string originJsonString = File.ReadAllText(filePath);
            string load = Load(filePath);

            return JsonToObject<DefenseFlowDataList>(load);
        }
        // PC에서 게임 실행
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Debug.Log("PC에서 실행");
            //string originJsonString = File.ReadAllText(filePath);
            string load = Load(filePath);

            return JsonToObject<DefenseFlowDataList>(load);
        }

        // 모바일 경로 체크
        else
        {
            string originPath = filePath;

            WWW reader = new WWW(originPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + ".Json";
            File.WriteAllBytes(realPath, reader.bytes);

            // string jsonString = File.ReadAllText(realPath);
            string load = Load(realPath);
            return JsonToObject<DefenseFlowDataList>(load);
        }
    }

    // JsonData만 읽어오는 메소드
    public static string ReadJsonFileToString(string filePath)
    {
        string jsonString;

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            jsonString = File.ReadAllText(filePath);

            // 복호화 추가
            // string load = Load(filePath);

            return jsonString;
        }

        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            jsonString = File.ReadAllText(filePath);

            // 복호화 추가
            //string load = Load(filePath);

            return jsonString;
        }

        else
        {
            string originPath = filePath;

            WWW reader = new WWW(originPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + ".Json";
            File.WriteAllBytes(realPath, reader.bytes);

            jsonString = File.ReadAllText(realPath);
            // string load = Load(realPath);
            return jsonString;
        }
    }

    // 암호화 메소드
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

    // 복호화 메소드
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

    public static void Save(string filePath)
    {
        JsonToDecrypt(filePath);
        string save = ReadJsonFileToString(filePath);
        save = Encrypt(save, "key");
        File.WriteAllText(filePath, save);
    }

    public static string Load(string filePath)
    {
        string load = File.ReadAllText(filePath);
        load = Decrypt(load, "key");
        return load;
    }

    public static void JsonToDecrypt(string filePath)
    {
        string load = File.ReadAllText(filePath);
        load = Decrypt(load, "key");
        File.WriteAllText(filePath, load);
    }

    //Json 새로 불러오기
    public static void LoadFromJson()
    {

    }

    //Json 삭제
    public static void DeleteJson()
    {

    }
}
