using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    public UserInfo userinfo = new UserInfo();
}
public class SaveLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();
    // Start is called before the first frame update
    void Start()
    {
        SaveData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData()
    {
        if (!File.Exists(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json")))
        {
            string json = JsonUtility.ToJson(saveData.userinfo);
            File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json"), json);
        }
        else
        {

        }



            
         

    }
    public static void Save(string filePath)
    {
        // Json 복호화
        EncryptDecrypt.Decrypt(filePath, "key");
        string save = File.ReadAllText(filePath);
        save = EncryptDecrypt.Encrypt(save, "key");
        // 암호화된 Json 저장
        File.WriteAllText(filePath, save);
    }

    public static string Load(string filePath)
    {
        string load = File.ReadAllText(filePath);
        load = EncryptDecrypt.Decrypt(load, "key");
        return load;
    }

    public void LoadUserInfo()
    {
        if (!File.Exists(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json"))){
            SaveData();
        }
            
    }
}
