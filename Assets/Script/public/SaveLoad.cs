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
        // SaveData();
        LoadUserInfo();
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
            saveData.userinfo = SaveUserInfoInitial(saveData.userinfo);
        }



            
         

    }
    public static void Save(string filePath)
    {
        // Json ��ȣȭ
        EncryptDecrypt.Decrypt(filePath, "key");
        string save = File.ReadAllText(filePath);
        save = EncryptDecrypt.Encrypt(save, "key");
        // ��ȣȭ�� Json ����
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
        if (!File.Exists(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json"))) {
            SaveData();
        } else {
            string data = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json"));
            saveData.userinfo = JsonUtility.FromJson<UserInfo>(data);

            Debug.Log("saveData : " + saveData.userinfo.isBgSound);
        }
            
    }

    // ���� �� UserInfo �ʱ�ȭ
    public UserInfo SaveUserInfoInitial(UserInfo userinfo)
    {
        // userinfo ��ȭ �ڿ� �ʱ�ȭ
        userinfo.colorWoodResource[0] = SystemManager.Instance.UserInfo.colorWoodResource[0];
        userinfo.colorWoodResource[1] = SystemManager.Instance.UserInfo.colorWoodResource[1];
        userinfo.colorWoodResource[2] = SystemManager.Instance.UserInfo.colorWoodResource[2];
        userinfo.colorWoodResource[3] = SystemManager.Instance.UserInfo.colorWoodResource[3];
        userinfo.colorWoodResource[4] = SystemManager.Instance.UserInfo.colorWoodResource[4];
        userinfo.colorWoodResource[5] = SystemManager.Instance.UserInfo.colorWoodResource[5];

        //  userStageInfo �ʱ�ȭ

        userinfo.maxStageNum = SystemManager.Instance.UserInfo.maxStageNum;
        userinfo.selectedStageNum = SystemManager.Instance.UserInfo.selectedStageNum;
        userinfo.stageStarList = SystemManager.Instance.UserInfo.stageStarList;

        // userTurretInfo �ʱ�ȭ
        userinfo.maxTurretNum = SystemManager.Instance.UserInfo.maxTurretNum;
        userinfo.turretPreset = SystemManager.Instance.UserInfo.turretPreset;

        //Option �ʱ�ȭ
        userinfo.bgSoundVolume = SystemManager.Instance.UserInfo.bgSoundVolume;
        userinfo.isBgSound = SystemManager.Instance.UserInfo.isBgSound;
        userinfo.efSoundVolume = SystemManager.Instance.UserInfo.efSoundVolume;
        userinfo.isEfSound = SystemManager.Instance.UserInfo.isEfSound;

        return userinfo;
    }

    public void LoadUserInfoInitial(UserInfo userinfo)
    {
        // userinfo ��ȭ �ڿ� �ҷ����� �ʱ�ȭ
        SystemManager.Instance.UserInfo.colorWoodResource[0] = userinfo.colorWoodResource[0];
        SystemManager.Instance.UserInfo.colorWoodResource[1] = userinfo.colorWoodResource[1];
        SystemManager.Instance.UserInfo.colorWoodResource[2] = userinfo.colorWoodResource[2];
        SystemManager.Instance.UserInfo.colorWoodResource[3] = userinfo.colorWoodResource[3];
        SystemManager.Instance.UserInfo.colorWoodResource[4] = userinfo.colorWoodResource[4];
        SystemManager.Instance.UserInfo.colorWoodResource[5] = userinfo.colorWoodResource[5];

        // userStageInfo �ҷ����� �ʱ�ȭ
        SystemManager.Instance.UserInfo.maxStageNum = userinfo.maxStageNum;
        SystemManager.Instance.UserInfo.selectedStageNum = userinfo.selectedStageNum;
        SystemManager.Instance.UserInfo.stageStarList = userinfo.stageStarList;

        // userTurretInfo �ҷ����� �ʱ�ȭ
        SystemManager.Instance.UserInfo.maxTurretNum = userinfo.maxTurretNum;
        SystemManager.Instance.UserInfo.turretPreset = userinfo.turretPreset;

        // Option �ҷ����� �ʱ�ȭ
        SystemManager.Instance.UserInfo.bgSoundVolume = userinfo.bgSoundVolume;
        SystemManager.Instance.UserInfo.isBgSound = userinfo.isBgSound;
        SystemManager.Instance.UserInfo.efSoundVolume = userinfo.efSoundVolume;
        SystemManager.Instance.UserInfo.isEfSound = userinfo.isEfSound;
    }
}
