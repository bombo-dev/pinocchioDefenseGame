using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;


public class EncryptJson : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 1. ���� ����Ʈ �����Ͱ� ��� JSON�� ��ȣȭ �� �� ���
        // JsonEncrypt(TestPath());
        // 2. ���͵��� ����ġ �����Ͱ� ��� JSON�� ��ȣȭ �� �� ���
        // JsonEncrypt(MonsterPath());
        // 3. Ÿ������ ����ġ �����Ͱ� ��� JSON�� ��ȣȭ �� �� ���
        // JsonEncrypt(TurretPath());
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

    // ��ȣȭ �� JSON ���� �ҷ�����
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

    //Json ��ȣȭ �ϱ�
    public static void JsonEncrypt(string filePath)
    {
        //JSON �����͸� ���� �о�ͼ� ���ڿ��� ����
        string encrypt = ReadJson(filePath);

        //JSON�� ��ȣȭ �Ҷ� ��ȣŰ�� �����ڵ鳢�� �� �� �ֵ��� ����
        encrypt = Encrypt(encrypt, "key");

        File.WriteAllText(filePath, encrypt);
    }

    // ��ȣȭ�� ���� �޼���
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
