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
        // 4. ��� Json ��ȣȭ
        // AllDecryptJson();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ���� ����Ʈ �����Ͱ� ��� JSON�� ���
    public string TestPath()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Spawn.Json");
        return filePath;
    }

    // ���͵��� ����ġ �����Ͱ� ��� JSON�� ���
    public string MonsterPath()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Enemy.Json");
        return filePath;
    }

    // �ͷ����� ����ġ �����Ͱ� ��� JSON�� ���
    public string TurretPath()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "turret.Json");
        return filePath;
    }

    public string ReadJson(string filePath)
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
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            jsonString = File.ReadAllText(filePath);

            return jsonString;
        }

        else if (Application.platform == RuntimePlatform.OSXPlayer)
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

    public string JsonDecrypt(string filePath)
    {
        //JSON �����͸� ���� �о�ͼ� ���ڿ��� ����
        string decrypt = ReadJson(filePath);

        // ��ȣȭŰ�� ��ȣȭ Ű�� �����ؾ� �Ѵ�.
        decrypt = EncryptDecrypt.Decrypt(decrypt, "chungwoonPinocchio");
        File.WriteAllText(filePath, decrypt);
        return decrypt;
    }
    // Json���� ���� ��ȣȭ
    public void AllDecryptJson()
    {
        JsonDecrypt(TestPath());
        JsonDecrypt(MonsterPath());
        JsonDecrypt(TurretPath());
    }
}
