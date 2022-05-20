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
        // Save(PathInit2());
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

        //Json ��� üũ
        filePath = PathInit();

        return LoadJsonFile<DefenseFlowDataList>(filePath);
    }

    //Json File �ʱ�ȭ
    public string PathInit()
    {
        string filePath;
            filePath = Path.Combine(Application.streamingAssetsPath, "Test.Json");
            return filePath;
    }

    // JsonData�� ��üȭ �޼ҵ�
    public static DefenseFlowDataList JsonToObject<DefenseFlowDataList>(string jsonString) 
    {
        return JsonUtility.FromJson<DefenseFlowDataList>(jsonString);
    }

    // JsonData �ҷ����� �޼ҵ�
    public static DefenseFlowDataList LoadJsonFile<DefenseFlowDataList>(string filePath) {

        // ������ ����Ƽ ������ ��� üũ
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            // ��ȣȭ �߰� �޼ҵ� *****************

            //string jsonString = File.ReadAllText(filePath);

            Debug.Log("����Ƽ �����Ϳ��� ����");

            string originJsonString = File.ReadAllText(filePath);
            // string load = Load(originJsonString, "Test.Json");

            return JsonToObject<DefenseFlowDataList>(originJsonString);
            //return JsonToObject<DefenseFlowDataList>(jsonString);
        }
        // PC���� ���� ����
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Debug.Log("PC���� ����");
            string originJsonString = File.ReadAllText(filePath);
            string load = Load(originJsonString, "Test.Json");

            return JsonToObject<DefenseFlowDataList>(load);
        }

        // ����� ��� üũ
        else
        {
            string originPath = filePath;

            WWW reader = new WWW(originPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + ".Json";
            File.WriteAllBytes(realPath, reader.bytes);

            string jsonString = File.ReadAllText(realPath);

            return JsonToObject<DefenseFlowDataList>(jsonString);
        }
    }

    // JsonData�� �о���� �޼ҵ�
    public static string ReadJsonFileToString(string filePath)
    {
        string jsonString;

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            jsonString = File.ReadAllText(filePath);

            // ��ȣȭ �߰�
            string load = Load(jsonString, "Test.Json");

            return load;
        }
        else
        {
            string originPath = Path.Combine(Application.streamingAssetsPath, "Test.Json");

            WWW reader = new WWW(originPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + ".Json";
            File.WriteAllBytes(realPath, reader.bytes);

            jsonString = File.ReadAllText(realPath);
            return jsonString;
        }
    }

    // ��ȣȭ �޼ҵ�
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

    // ��ȣȭ �޼ҵ�
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
        string load = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "jsonAssets.json"));
        load = Decrypt(load, "key");
        File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "jsonAssets.json"), load);
    }

}
