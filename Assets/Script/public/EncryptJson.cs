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
        // ****** ��ȣȭ �Ҷ��� ������ ��ȣȭ �Ǿ� �ִ� ���¿��� �մϴ� . ��ȣȭ �� ������ �� �� ��ȣȭ �ϸ� ������ ���̴� ���� ���� ��ȣȭ �� JSON ���� ������ּ��� ******

        // 1. ���� ����Ʈ �����Ͱ� ��� JSON�� ��ȣȭ �� �� ���
        // JsonEncrypt(TestPath());
        // 2. ���͵��� ����ġ �����Ͱ� ��� JSON�� ��ȣȭ �� �� ���
        // JsonEncrypt(MonsterPath());
        // 3. Ÿ������ ����ġ �����Ͱ� ��� JSON�� ��ȣȭ �� �� ���
        // JsonEncrypt(TurretPath());
        // 4. ��� Json ������ ��ȣȭ
        // AllEncryptJson();
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

    // ��ȣȭ �� JSON ���� �ҷ�����
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

    //Json ��ȣȭ �ϱ�
    public void JsonEncrypt(string filePath)
    {
        //JSON �����͸� ���� �о�ͼ� ���ڿ��� ����
        string encrypt = ReadJson(filePath);

        //JSON�� ��ȣȭ �Ҷ� ��ȣŰ�� �����ڵ鳢�� �� �� �ֵ��� ����
        encrypt = EncryptDecrypt.Encrypt(encrypt, "chungwoonPinocchio");

        File.WriteAllText(filePath, encrypt);
    }

    //Json ��ü ���� ��ȣȭ
    public void AllEncryptJson()
    {
        JsonEncrypt(TestPath());
        JsonEncrypt(MonsterPath());
        JsonEncrypt(TurretPath());
    }
}
