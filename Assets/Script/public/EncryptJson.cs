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
        // 1. 몬스터 게이트 데이터가 담긴 JSON을 암호화 할 때 사용
        // JsonEncrypt(TestPath());
        // 2. 몬스터들의 상태치 데이터가 담긴 JSON을 암호화 할 때 사용
        // JsonEncrypt(MonsterPath());
        // 3. 타워들의 상태치 데이터가 담긴 JSON을 암호화 할 때 사용
        // JsonEncrypt(TurretPath());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 몬스터 게이트 데이터가 담긴 JSON의 경로
    public string TestPath()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Test.Json");
        return filePath;
    }

    // 몬스터들의 상태치 데이터가 담긴 JSON의 경로
    public string MonsterPath()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Monster.Json");
        return filePath;
    }

    // 터렛들의 상태치 데이터가 담긴 JSON의 경로
    public string TurretPath()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "turret.Json");
        return filePath;
    }

    // 암호화 할 JSON 파일 불러오기
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

    //Json 암호화 하기
    public static void JsonEncrypt(string filePath)
    {
        //JSON 데이터를 먼저 읽어와서 문자열로 저장
        string encrypt = ReadJson(filePath);

        //JSON을 암호화 할때 암호키는 개발자들끼리 알 수 있도록 지정
        encrypt = Encrypt(encrypt, "key");

        File.WriteAllText(filePath, encrypt);
    }

    // 암호화를 위한 메서드
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
