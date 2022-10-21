using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager instance = null;
    //싱글톤 프로퍼티
    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }

    //오디오 소스
    public AudioSource audioSource;

    [Header("BG_Sound")]
    //사운드 클립
    [SerializeField]
    AudioClip lobbySceneAudioClip;
    [SerializeField]
    AudioClip []gameSceneAuduiClip;
    [SerializeField]
    AudioClip stroySceneAudioClip;

    [SerializeField]
    AudioClip failAudioClip;
    [SerializeField]
    AudioClip winAudioClip;

    void Awake()
    {
        //유일한 instance
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        //Scene이동간에 사라지지 않도록 처리
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeBGAudioClip(int sceneBuildIndex)
    {
        switch (sceneBuildIndex)
        {
            //로비씬
            //로딩씬
            case 0:
            case 1:
                //Clip교체
                audioSource.clip = lobbySceneAudioClip;
                //재생
                audioSource.Play();
                break;
            //게임씬
            case 2:
                int stage;
                if (SystemManager.Instance.UserInfo.selectMode == 0)    //노말
                    stage = SystemManager.Instance.UserInfo.selectedStageNum;
                else   //하드
                    stage = SystemManager.Instance.UserInfo.selectedStageNum_hard;

                //Clip교체
                if (stage <= 20)
                    audioSource.clip = gameSceneAuduiClip[0];
                else if (stage <= 25)
                    audioSource.clip = gameSceneAuduiClip[1];
                else if (stage <= 35)
                    audioSource.clip = gameSceneAuduiClip[2];
                else if (stage <= 39)
                    audioSource.clip = gameSceneAuduiClip[3];
                else
                    audioSource.clip = gameSceneAuduiClip[4];
                //재생
                audioSource.Play();
                break;
            //스토리씬
            case 3:
                //Clip교체
                audioSource.clip = stroySceneAudioClip;
                //재생
                audioSource.Play();
                break;
            //승리 
            case 4:
                //Clip교체
                audioSource.clip = winAudioClip;
                //재생
                audioSource.Play();
                break;
            //패배
            case 5:
                //Clip교체
                audioSource.clip = failAudioClip;
                //재생
                audioSource.Play();
                break;
        }
    }
}
