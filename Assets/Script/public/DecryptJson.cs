using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class DecryptJson : MonoBehaviour
{
    /// <summary>
    /// *** JSON�� ��ȣȭ �� ���� ��ȣȭ�� �Ǿ��־�� �մϴ� ���� ***
    /// </summary>
    void Start()
    {
        // ********** JSON�� ��ȣȭ �� ���� ��ȣȭ�� �Ǿ��־�� �մϴ� ���� ************

        // 1. ���� ����Ʈ �����Ͱ� ��� JSON�� ��ȣȭ �� �� ���
        // JsonDecrypt(TestPath());
        // 2. ���͵��� ����ġ �����Ͱ� ��� JSON�� ��ȣȭ �� �� ���
        // JsonDecrypt(MonsterPath());
        // 3. Ÿ������ ����ġ �����Ͱ� ��� JSON�� ��ȣȭ �� �� ���
        // JsonDecrypt(TurretPath());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ���� ����Ʈ �����Ͱ� ��� JSON�� ���
    public string TestPath()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Test.Json");
        return filePath;
    }

    // ���͵��� ����ġ �����Ͱ� ��� JSON�� ���
    public string MonsterPath()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Monster.Json");
        return filePath;
    }

    // �ͷ����� ����ġ �����Ͱ� ��� JSON�� ���
    public string TurretPath()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "turret.Json");
        return filePath;
    }

    public static string ReadJson(string filePath)
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

            WWW reader = new WWW(originPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + ".Json";
            File.WriteAllBytes(realPath, reader.bytes);

            jsonString = File.ReadAllText(realPath);
            return jsonString;
        }

    }

    public static string JsonDecrypt(string filePath)
    {
        //JSON �����͸� ���� �о�ͼ� ���ڿ��� ����
        string decrypt = ReadJson(filePath);

        // ��ȣȭŰ�� ��ȣȭ Ű�� �����ؾ� �Ѵ�.
        decrypt = Decrypt(decrypt, "key"); 
        return decrypt;
    }

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

}
