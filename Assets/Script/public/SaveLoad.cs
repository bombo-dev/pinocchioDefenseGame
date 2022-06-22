using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [Header("WoodIResourceInfo")]    //��ȭ ���� �ڿ�
    public int[] colorWoodResource = new int[6]; //0 - red, 1- yellow, 2 - green, 3 - white, 4 - blue, 5 - black

    [Header("StageInfo")]   //�������� ����
    public int maxStageNum; //�ִ� Ŭ������ ��������
    public int selectedStageNum;   //������ ��������

    public List<StageStar> stageStarList;    //�������� Ŭ���� �� ����

    [Header("TurretInfo")]
    public int maxTurretNum;    //�ִ� �ͷ� ����
    public List<int> turretPreset; //���õ� �ͷ� ����Ʈ

    [Header("Option")]
    public float bgSoundVolume;
    public bool isBgSound;
    public float efSoundVolume;
    public bool isEfSound;
}

public class SaveLoad
{
    private SaveData saveData = new SaveData();
    // Start is called before the first frame update
    void Start()
    {
        // SaveData();
        // LoadUserInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData()
    {
        if (!File.Exists(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json")))
        {
            saveData = SaveConstructorUserInfo(saveData, new UserInfo());
            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json"), json);
        }
        else
        {
            saveData = SaveUserInfoInitial(saveData);
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
        Debug.Log("Userinfo�� �ε��߽��ϴ�.");
        if (!File.Exists(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json"))) {
            SaveData();
            Debug.Log("������ ����, Userinfo�� �����ϰ� �ʱ�ȭ�մϴ�.");
            Debug.Log("UserInfo: isBgSound" + saveData.isBgSound);
        } else {
            string data = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json"));
            saveData = JsonUtility.FromJson<SaveData>(data);

            Debug.Log("�����Ͱ� �����߰�, ���� �����ͷ� �ʱ�ȭ�Ͽ� �ҷ��ɴϴ�.");

            LoadUserInfoInitial(saveData);
            
            Debug.Log("UserInfo : isBgSound" + SystemManager.Instance.UserInfo.isBgSound);
        }
            
    }
    // UserInfo.Json ���� ���� �� UserInfo �ʱ�ȭ
    public SaveData SaveConstructorUserInfo(SaveData data, UserInfo userinfo)
    {
        // userinfo ����
        data.colorWoodResource[0] = userinfo.colorWoodResource[0];
        data.colorWoodResource[1] = userinfo.colorWoodResource[1];
        data.colorWoodResource[2] = userinfo.colorWoodResource[2];
        data.colorWoodResource[3] = userinfo.colorWoodResource[3];
        data.colorWoodResource[4] = userinfo.colorWoodResource[4];
        data.colorWoodResource[5] = userinfo.colorWoodResource[5];

        // userStageInfo ����
        data.maxStageNum = userinfo.maxStageNum;
        data.selectedStageNum = userinfo.selectedStageNum;
        data.stageStarList = userinfo.stageStarList;

        // userTurretInfo ����
        data.maxTurretNum = userinfo.maxTurretNum;
        data.turretPreset = userinfo.turretPreset;

        // Option ����
        data.bgSoundVolume = userinfo.bgSoundVolume;
        data.isBgSound = userinfo.isBgSound;
        data.efSoundVolume = userinfo.efSoundVolume;
        data.isEfSound = userinfo.isEfSound;

        return data;
    }

    // Json ���� ���� �� ���� �� UserInfo �ʱ�ȭ
    public SaveData SaveUserInfoInitial(SaveData data)
    {
        // userinfo ��ȭ �ڿ� �ʱ�ȭ
        data.colorWoodResource[0] = SystemManager.Instance.UserInfo.colorWoodResource[0];
        data.colorWoodResource[1] = SystemManager.Instance.UserInfo.colorWoodResource[1];
        data.colorWoodResource[2] = SystemManager.Instance.UserInfo.colorWoodResource[2];
        data.colorWoodResource[3] = SystemManager.Instance.UserInfo.colorWoodResource[3];
        data.colorWoodResource[4] = SystemManager.Instance.UserInfo.colorWoodResource[4];
        data.colorWoodResource[5] = SystemManager.Instance.UserInfo.colorWoodResource[5];

        //  userStageInfo �ʱ�ȭ

        data.maxStageNum = SystemManager.Instance.UserInfo.maxStageNum;
        data.selectedStageNum = SystemManager.Instance.UserInfo.selectedStageNum;
        data.stageStarList = SystemManager.Instance.UserInfo.stageStarList;

        // userTurretInfo �ʱ�ȭ
        data.maxTurretNum = SystemManager.Instance.UserInfo.maxTurretNum;
        data.turretPreset = SystemManager.Instance.UserInfo.turretPreset;

        //Option �ʱ�ȭ
        data.bgSoundVolume = SystemManager.Instance.UserInfo.bgSoundVolume;
        data.isBgSound = SystemManager.Instance.UserInfo.isBgSound;
        data.efSoundVolume = SystemManager.Instance.UserInfo.efSoundVolume;
        data.isEfSound = SystemManager.Instance.UserInfo.isEfSound;

        return data;
    }

    public void LoadUserInfoInitial(SaveData data)
    {
        // userinfo ��ȭ �ڿ� �ҷ����� �ʱ�ȭ
        SystemManager.Instance.UserInfo.colorWoodResource[0] = data.colorWoodResource[0];
        SystemManager.Instance.UserInfo.colorWoodResource[1] = data.colorWoodResource[1];
        SystemManager.Instance.UserInfo.colorWoodResource[2] = data.colorWoodResource[2];
        SystemManager.Instance.UserInfo.colorWoodResource[3] = data.colorWoodResource[3];
        SystemManager.Instance.UserInfo.colorWoodResource[4] = data.colorWoodResource[4];
        SystemManager.Instance.UserInfo.colorWoodResource[5] = data.colorWoodResource[5];

        // userStageInfo �ҷ����� �ʱ�ȭ
        SystemManager.Instance.UserInfo.maxStageNum = data.maxStageNum;
        SystemManager.Instance.UserInfo.selectedStageNum = data.selectedStageNum;
        SystemManager.Instance.UserInfo.stageStarList = data.stageStarList;

        // userTurretInfo �ҷ����� �ʱ�ȭ
        SystemManager.Instance.UserInfo.maxTurretNum = data.maxTurretNum;
        SystemManager.Instance.UserInfo.turretPreset = data.turretPreset;

        // Option �ҷ����� �ʱ�ȭ
        SystemManager.Instance.UserInfo.bgSoundVolume = data.bgSoundVolume;
        SystemManager.Instance.UserInfo.isBgSound = data.isBgSound;
        SystemManager.Instance.UserInfo.efSoundVolume = data.efSoundVolume;
        SystemManager.Instance.UserInfo.isEfSound = data.isEfSound;
    }

    
}
