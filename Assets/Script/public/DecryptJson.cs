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
    /// *** JSON을 복호화 할 때는 암호화가 되어있어야 합니다 꼭꼭 ***
    /// </summary>
    void Start()
    {
        // ********** JSON을 복호화 할 때는 암호화가 되어있어야 합니다 꼭꼭 ************

        // 1. 몬스터 게이트 데이터가 담긴 JSON을 복호화 할 때 사용
        // JsonDecrypt(TestPath());
        // 2. 몬스터들의 상태치 데이터가 담긴 JSON을 복호화 할 때 사용
        // JsonDecrypt(MonsterPath());
        // 3. 타워들의 상태치 데이터가 담긴 JSON을 복호화 할 때 사용
        // JsonDecrypt(TurretPath());
        // 4. 모든 Json 복호화
        // AllDecryptJson();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 몬스터 게이트 데이터가 담긴 JSON의 경로
    public string TestPath()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Spawn.Json");
        return filePath;
    }

    // 몬스터들의 상태치 데이터가 담긴 JSON의 경로
    public string MonsterPath()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Enemy.Json");
        return filePath;
    }

    // 터렛들의 상태치 데이터가 담긴 JSON의 경로
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
        //JSON 데이터를 먼저 읽어와서 문자열로 저장
        string decrypt = ReadJson(filePath);

        // 복호화키는 암호화 키와 동일해야 한다.
        decrypt = EncryptDecrypt.Decrypt(decrypt, "chungwoonPinocchio");
        File.WriteAllText(filePath, decrypt);
        return decrypt;
    }
    // Json파일 전부 복호화
    public void AllDecryptJson()
    {
        JsonDecrypt(TestPath());
        JsonDecrypt(MonsterPath());
        JsonDecrypt(TurretPath());
    }
}
