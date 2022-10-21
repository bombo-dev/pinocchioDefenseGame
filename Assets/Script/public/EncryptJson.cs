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
        // ****** 암호화 할때는 파일이 복호화 되어 있는 상태여야 합니다 . 암호화 된 파일을 두 번 암호화 하면 파일이 꼬이니 실행 전에 복호화 된 JSON 파일 백업해주세요 ******

        // 1. 몬스터 게이트 데이터가 담긴 JSON을 암호화 할 때 사용
        // JsonEncrypt(TestPath());
        // 2. 몬스터들의 상태치 데이터가 담긴 JSON을 암호화 할 때 사용
        // JsonEncrypt(MonsterPath());
        // 3. 타워들의 상태치 데이터가 담긴 JSON을 암호화 할 때 사용
        // JsonEncrypt(TurretPath());
        // 4. 모든 Json 데이터 암호화
        // AllEncryptJson();
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

    // 암호화 할 JSON 파일 불러오기
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

    //Json 암호화 하기
    public void JsonEncrypt(string filePath)
    {
        //JSON 데이터를 먼저 읽어와서 문자열로 저장
        string encrypt = ReadJson(filePath);

        //JSON을 암호화 할때 암호키는 개발자들끼리 알 수 있도록 지정
        encrypt = EncryptDecrypt.Encrypt(encrypt, "chungwoonPinocchio");

        File.WriteAllText(filePath, encrypt);
    }

    //Json 전체 파일 암호화
    public void AllEncryptJson()
    {
        JsonEncrypt(TestPath());
        JsonEncrypt(MonsterPath());
        JsonEncrypt(TurretPath());
    }
}
