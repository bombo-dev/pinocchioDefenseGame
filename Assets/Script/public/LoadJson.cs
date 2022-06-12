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
        
        // *************** ��ȣȭ �Ǿ��ִ� JSON �޼ҵ� ���� *****************************
        //Save(PathInit()); // ��ȣȭ �� Json �ҷ����� ����
        //PrepareGameFlowJsonData(); // ��ȣȭ �� Json ������ �ҷ��� ����
        // ********************************************************************************


        // **************** ��ȣȭ �Ǿ����� ���� Json �޼ҵ� ���� ************************
        // PrepareGameFlowDecryptJsonData();
        // ********************************************************************************
    }

    /// <summary>
    /// Json ���Ϸκ��� ��ȣȭ �Ǿ��ִ� Json ������ �������� 
    /// </summary>
    public DefenseFlowDataList PrepareGameFlowJsonData()
    {
        DefenseFlowData[] defenseFlowDatas = new DefenseFlowData[3];

        //Json �ҷ�����
        string filePath = Path.Combine(Application.streamingAssetsPath, "Test.Json");

        return LoadJsonFile<DefenseFlowDataList>(filePath);
    }


    /// <summary>
	/// Json ���Ϸκ��� ��ȣȭ �Ǿ��ִ� Json ������ �������� 
	/// </summary>
	
    public DefenseFlowDataList PrepareGameFlowDecryptJsonData()
    {
        DefenseFlowData[] defenseFlowDatas = new DefenseFlowData[3];

        //Json �ҷ�����
        string filePath = Path.Combine(Application.streamingAssetsPath, "Test.Json");

        return DecryptLoadJsonFile<DefenseFlowDataList>(filePath);

    }

    //Json filePath ��� �ʱ�ȭ
    public string PathInit()
    {
            string filePath;
            filePath = Path.Combine(Application.streamingAssetsPath, "Test.Json");
            return filePath;
    }

    // JsonData�� ��üȭ �޼ҵ�
    public DefenseFlowDataList JsonToObject<DefenseFlowDataList>(string jsonString) 
    {
        return JsonUtility.FromJson<DefenseFlowDataList>(jsonString);
    }

    // JsonData �ҷ����� �޼ҵ�
    public DefenseFlowDataList LoadJsonFile<DefenseFlowDataList>(string filePath) {

        // ������ ����Ƽ ������ ���
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("����Ƽ �����Ϳ��� ����");

            //string jsonString = File.ReadAllText(filePath);

            // ��ȣȭ �Ǿ� �ִ� JsonData ��ȣȭ�ؼ� ����
            string load = Load(filePath);

            return JsonToObject<DefenseFlowDataList>(load);
        }
        // Windows PC���� ���� ����
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Debug.Log("PC���� ����");


            // string jsonString = File.ReadAllText(filePath);
            // ��ȣȭ �Ǿ� �ִ� JsonData ��ȣȭ�ؼ� ����
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
            string load = Load(filePath);

            return JsonToObject<DefenseFlowDataList>(load);
        }

        // ����� ��� üũ
        else
        {
            string originPath = filePath;
            #pragma warning disable 612, 618
            WWW reader = new WWW(originPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + ".Json";
            File.WriteAllBytes(realPath, reader.bytes);


            string jsonString = File.ReadAllText(realPath);
            //��ȣȭ �Ǿ� �ִ� JsonData ��ȣȭ�ؼ� ����
            //string load = Load(realPath);
            return JsonToObject<DefenseFlowDataList>(jsonString);
        }
    }

    // ��ȣȭ ���� ���� JsonData �ҷ����� �޼ҵ�
    public DefenseFlowDataList DecryptLoadJsonFile<DefenseFlowDataList>(string filePath)
    {

        // ������ ����Ƽ ������ ��� üũ
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("����Ƽ �����Ϳ��� ����");

            string jsonString = File.ReadAllText(filePath);

            return JsonToObject<DefenseFlowDataList>(jsonString);
        }
        // PC���� ���� ����
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Debug.Log("PC���� ����");

            string jsonString = File.ReadAllText(filePath);
            
            return JsonToObject<DefenseFlowDataList>(jsonString);
        }

        // MAC OS Editor���� ����
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            Debug.Log("�� OS Editor���� ����");
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

        // ����� ��� üũ
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
    
    // JsonData�� �о���� �޼ҵ�
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
        // ��ȣȭ �� Json ���� ��ȣȭ
        JsonToDecrypt(filePath);
        string save = ReadJsonFileToString(filePath);
        save = Encrypt(save, "key");
        // Json ���� ��ȣȭ �Ͽ� �� ����
        File.WriteAllText(filePath, save);
    }

    public string Load(string filePath)
    {
        string load = File.ReadAllText(filePath);
        load = Decrypt(load, "key");
        return load;
    }

    public void JsonToDecrypt(string filePath)
    {
        string load = File.ReadAllText(filePath);
        load = Decrypt(load, "key");
        File.WriteAllText(filePath, load);
    }



    /// <summary>
    /// ��ȣȭ, ��ȣȭ ���� �޼ҵ� ����
    /// </summary>
    /// <returns></returns>


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

}
