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
        //JSON 데이터를 먼저 읽어와서 문자열로 저장
        string decrypt = ReadJson(filePath);

        // 복호화키는 암호화 키와 동일해야 한다.
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
