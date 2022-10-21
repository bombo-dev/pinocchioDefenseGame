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
    public int maxStageNum_hard; //최대 클리어한 스테이지 - 하드
    public int selectedStageNum_hard;   //선택한 스테이지 - 하드
    public int selectMode;  //선택한 모드 0 - 노말, 1 - 하드

    public List<StageStar> stageStarList;    //스테이지 클리어 별 정보
    public List<StageStar> stageStarList_hard;    //스테이지 클리어 별 정보 - 하드

    [Header("TurretInfo")]
    public int maxTurretNum;    //최대 터렛 숫자
    public List<int> turretPreset; //선택된 터렛 리스트

    [Header("Option")]
    public float bgSoundVolume;
    public bool isBgSound;
    public float efSoundVolume;
    public bool isEfSound;
    public int touchSpeed;
    public bool isShowRange;
    public bool isShowBook;
}

public class SaveLoad
{
    private SaveData saveData = new SaveData();
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveUserInfo()
    {
        // PC이고 Windows유니티 Editor에서 실행하는 경우에 세이브 
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            PCSave();
        }
        // PC이고 Windows에서 실행하는 경우에 세이브
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            PCSave();
        }
        // PC이고 맥OS 유니티 편집기에서 실행하는 경우에 세이브
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            PCSave();
        }
        // PC이고 맥OS에서 실행하는 경우에 세이브
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            PCSave();
        }
        // 안드로이드에서 실행
        else
        {
            MobileSave();
        }
        
    }

    public void LoadUserInfo()
    {
        // PC이고 Windows 유니티 Editor에서 실행하는 경우에 Load
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            PCLoad();
        }
        // PC이고 Windows에서 실행하는 경우 Load
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            PCLoad();
        }
        // PC이고 맥 OS 유니티 편집기에서 실행하는 경우에 Load
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            PCLoad();
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            PCLoad();
        }
        else
        {
            MobileLoad();
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

        data.maxStageNum_hard = userinfo.maxStageNum_hard;
        data.selectedStageNum_hard = userinfo.selectedStageNum_hard;
        data.stageStarList_hard = userinfo.stageStarList_hard;

        data.selectMode = userinfo.selectMode;

        // userTurretInfo 생성
        data.maxTurretNum = userinfo.maxTurretNum;
        data.turretPreset = userinfo.turretPreset;

        // Option 생성
        data.bgSoundVolume = userinfo.bgSoundVolume;
        data.isBgSound = userinfo.isBgSound;
        data.efSoundVolume = userinfo.efSoundVolume;
        data.isEfSound = userinfo.isEfSound;
        data.touchSpeed = userinfo.touchSpeed;
        data.isShowRange = userinfo.isShowRange;
        data.isShowBook = userinfo.isShowBook;

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

        data.maxStageNum_hard = SystemManager.Instance.UserInfo.maxStageNum_hard;
        data.selectedStageNum_hard = SystemManager.Instance.UserInfo.selectedStageNum_hard;
        data.stageStarList_hard = SystemManager.Instance.UserInfo.stageStarList_hard;

        data.selectMode = SystemManager.Instance.UserInfo.selectMode;

        // userTurretInfo 초기화
        data.maxTurretNum = SystemManager.Instance.UserInfo.maxTurretNum;
        data.turretPreset = SystemManager.Instance.UserInfo.turretPreset;

        //Option 초기화
        data.bgSoundVolume = SystemManager.Instance.UserInfo.bgSoundVolume;
        data.isBgSound = SystemManager.Instance.UserInfo.isBgSound;
        data.efSoundVolume = SystemManager.Instance.UserInfo.efSoundVolume;
        data.isEfSound = SystemManager.Instance.UserInfo.isEfSound;
        data.touchSpeed = SystemManager.Instance.UserInfo.touchSpeed;
        data.isShowRange = SystemManager.Instance.UserInfo.isShowRange;
        data.isShowBook = SystemManager.Instance.UserInfo.isShowBook;

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

        SystemManager.Instance.UserInfo.maxStageNum_hard = data.maxStageNum_hard;
        SystemManager.Instance.UserInfo.selectedStageNum_hard = data.selectedStageNum_hard;
        SystemManager.Instance.UserInfo.stageStarList_hard = data.stageStarList_hard;

        SystemManager.Instance.UserInfo.selectMode = data.selectMode;

        // userTurretInfo 불러오기 초기화
        SystemManager.Instance.UserInfo.maxTurretNum = data.maxTurretNum;
        SystemManager.Instance.UserInfo.turretPreset = data.turretPreset;

        // Option 불러오기 초기화
        SystemManager.Instance.UserInfo.bgSoundVolume = data.bgSoundVolume;
        SystemManager.Instance.UserInfo.isBgSound = data.isBgSound;
        SystemManager.Instance.UserInfo.efSoundVolume = data.efSoundVolume;
        SystemManager.Instance.UserInfo.isEfSound = data.isEfSound;
        SystemManager.Instance.UserInfo.touchSpeed = data.touchSpeed;
        SystemManager.Instance.UserInfo.isShowRange = data.isShowRange;
        SystemManager.Instance.UserInfo.isShowBook = data.isShowBook;
    }

    // PC에서 Save 하는 경우
    public void PCSave()
    {
        // StreamingAssets에 파일 있는지 확인
        // 파일이 없으면 새로 생성
        if (!File.Exists(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json")))
        {
            saveData = SaveConstructorUserInfo(saveData, new UserInfo());
            string json = JsonUtility.ToJson(saveData);
            // json 암호화해서 저장
            // json = EncryptDecrypt.Encrypt(json, "chungwoonPinocchio");
            File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json"), json);
        }
        // 있으면 streamingAssets에 기존 데이터 가져와 파일 생성
        else
        {
            saveData = SaveUserInfoInitial(saveData);
            string json = JsonUtility.ToJson(saveData);
            // json 암호화해서 저장
            // json = EncryptDecrypt.Encrypt(json, "chungwoonPinocchio");
            File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "UserInfo.json"), json);
        }
    }
    // Mobile에서 Save 하는 경우
    public void MobileSave()
    {
        Debug.Log("모바일에서 Userinfo를 세이브했습니다.");
        //세이브 파일이 없으면
        if (!File.Exists(Application.persistentDataPath + "UserInfo.Json"))
        {
            saveData = SaveConstructorUserInfo(saveData, new UserInfo());
            string json = JsonUtility.ToJson(saveData);

            // json 암호화해서 저장
            // json = EncryptDecrypt.Encrypt(json, "chungwoonPinocchio");

            string realPath = Application.persistentDataPath + "UserInfo.Json";
            File.WriteAllText(realPath, json);

        }
        // 세이브 파일이 있으면
        else
        {
            saveData = SaveUserInfoInitial(saveData);
            string json = JsonUtility.ToJson(saveData);

            // json 암호화해서 저장
            // json = EncryptDecrypt.Encrypt(json, "chungwoonPinocchio");

            // 모바일 저장 공간에 따로 저장
            string realPath = Application.persistentDataPath + "UserInfo.Json";
            File.WriteAllText(realPath, json);
        }
    }

    // PC에서 Load 하는 경우
    public void PCLoad()
    {
        //파일이 없으면
        if (!File.Exists(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json")))
        {
            SaveUserInfo();
        }

        //파일이 있으면
        else
        {
            string data = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "UserInfo.Json"));
            // 암호화된 json 복호화
            // data = EncryptDecrypt.Decrypt(data, "chungwoonPinocchio");
            saveData = JsonUtility.FromJson<SaveData>(data);

            LoadUserInfoInitial(saveData);
        }
    }

    // Mobile에서 Load하는 경우
    public void MobileLoad()
    {
        Debug.Log("모바일에서 Userinfo를 로드했습니다.");

        if (!File.Exists(Application.persistentDataPath + "UserInfo.Json"))
        {
            SaveUserInfo();
        }

        else
        {
            string data = File.ReadAllText(Application.persistentDataPath+ "UserInfo.Json");
            // 암호화된 json 복호화
            // data = EncryptDecrypt.Decrypt(data, "chungwoonPinocchio");
            saveData = JsonUtility.FromJson<SaveData>(data);

            LoadUserInfoInitial(saveData);
        }
    }

    
}
