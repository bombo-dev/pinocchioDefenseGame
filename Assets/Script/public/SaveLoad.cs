using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [Header("WoodIResourceInfo")]    //강화 나무 자원
    public int[] colorWoodResource = new int[6]; //0 - red, 1- yellow, 2 - green, 3 - white, 4 - blue, 5 - black

    [Header("StageInfo")]   //스테이지 정보
    public int maxStageNum; //최대 클리어한 스테이지
    public int selectedStageNum;   //선택한 스테이지

    public List<StageStar> stageStarList;    //스테이지 클리어 별 정보

    [Header("TurretInfo")]
    public int maxTurretNum;    //최대 터렛 숫자
    public List<int> turretPreset; //선택된 터렛 리스트

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
        Debug.Log("Userinfo를 로드했습니다.");
        if (!File.Exists(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json"))) {
            SaveData();
            Debug.Log("파일이 없어, Userinfo를 생성하고 초기화합니다.");
            Debug.Log("UserInfo: isBgSound" + saveData.isBgSound);
        } else {
            string data = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json"));
            saveData = JsonUtility.FromJson<SaveData>(data);

            Debug.Log("데이터가 존재했고, 이전 데이터로 초기화하여 불러옵니다.");

            LoadUserInfoInitial(saveData);
            
            Debug.Log("UserInfo : isBgSound" + SystemManager.Instance.UserInfo.isBgSound);
        }
            
    }
    // UserInfo.Json 파일 없을 시 UserInfo 초기화
    public SaveData SaveConstructorUserInfo(SaveData data, UserInfo userinfo)
    {
        // userinfo 생성
        data.colorWoodResource[0] = userinfo.colorWoodResource[0];
        data.colorWoodResource[1] = userinfo.colorWoodResource[1];
        data.colorWoodResource[2] = userinfo.colorWoodResource[2];
        data.colorWoodResource[3] = userinfo.colorWoodResource[3];
        data.colorWoodResource[4] = userinfo.colorWoodResource[4];
        data.colorWoodResource[5] = userinfo.colorWoodResource[5];

        // userStageInfo 생성
        data.maxStageNum = userinfo.maxStageNum;
        data.selectedStageNum = userinfo.selectedStageNum;
        data.stageStarList = userinfo.stageStarList;

        // userTurretInfo 생성
        data.maxTurretNum = userinfo.maxTurretNum;
        data.turretPreset = userinfo.turretPreset;

        // Option 생성
        data.bgSoundVolume = userinfo.bgSoundVolume;
        data.isBgSound = userinfo.isBgSound;
        data.efSoundVolume = userinfo.efSoundVolume;
        data.isEfSound = userinfo.isEfSound;

        return data;
    }

    // Json 파일 있을 때 저장 전 UserInfo 초기화
    public SaveData SaveUserInfoInitial(SaveData data)
    {
        // userinfo 강화 자원 초기화
        data.colorWoodResource[0] = SystemManager.Instance.UserInfo.colorWoodResource[0];
        data.colorWoodResource[1] = SystemManager.Instance.UserInfo.colorWoodResource[1];
        data.colorWoodResource[2] = SystemManager.Instance.UserInfo.colorWoodResource[2];
        data.colorWoodResource[3] = SystemManager.Instance.UserInfo.colorWoodResource[3];
        data.colorWoodResource[4] = SystemManager.Instance.UserInfo.colorWoodResource[4];
        data.colorWoodResource[5] = SystemManager.Instance.UserInfo.colorWoodResource[5];

        //  userStageInfo 초기화

        data.maxStageNum = SystemManager.Instance.UserInfo.maxStageNum;
        data.selectedStageNum = SystemManager.Instance.UserInfo.selectedStageNum;
        data.stageStarList = SystemManager.Instance.UserInfo.stageStarList;

        // userTurretInfo 초기화
        data.maxTurretNum = SystemManager.Instance.UserInfo.maxTurretNum;
        data.turretPreset = SystemManager.Instance.UserInfo.turretPreset;

        //Option 초기화
        data.bgSoundVolume = SystemManager.Instance.UserInfo.bgSoundVolume;
        data.isBgSound = SystemManager.Instance.UserInfo.isBgSound;
        data.efSoundVolume = SystemManager.Instance.UserInfo.efSoundVolume;
        data.isEfSound = SystemManager.Instance.UserInfo.isEfSound;

        return data;
    }

    public void LoadUserInfoInitial(SaveData data)
    {
        // userinfo 강화 자원 불러오기 초기화
        SystemManager.Instance.UserInfo.colorWoodResource[0] = data.colorWoodResource[0];
        SystemManager.Instance.UserInfo.colorWoodResource[1] = data.colorWoodResource[1];
        SystemManager.Instance.UserInfo.colorWoodResource[2] = data.colorWoodResource[2];
        SystemManager.Instance.UserInfo.colorWoodResource[3] = data.colorWoodResource[3];
        SystemManager.Instance.UserInfo.colorWoodResource[4] = data.colorWoodResource[4];
        SystemManager.Instance.UserInfo.colorWoodResource[5] = data.colorWoodResource[5];

        // userStageInfo 불러오기 초기화
        SystemManager.Instance.UserInfo.maxStageNum = data.maxStageNum;
        SystemManager.Instance.UserInfo.selectedStageNum = data.selectedStageNum;
        SystemManager.Instance.UserInfo.stageStarList = data.stageStarList;

        // userTurretInfo 불러오기 초기화
        SystemManager.Instance.UserInfo.maxTurretNum = data.maxTurretNum;
        SystemManager.Instance.UserInfo.turretPreset = data.turretPreset;

        // Option 불러오기 초기화
        SystemManager.Instance.UserInfo.bgSoundVolume = data.bgSoundVolume;
        SystemManager.Instance.UserInfo.isBgSound = data.isBgSound;
        SystemManager.Instance.UserInfo.efSoundVolume = data.efSoundVolume;
        SystemManager.Instance.UserInfo.isEfSound = data.isEfSound;
    }

    
}
