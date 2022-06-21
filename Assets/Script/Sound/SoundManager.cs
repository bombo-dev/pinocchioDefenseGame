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
    [SerializeField]
    AudioSource audioSource;

    [Header("BG_Sound")]
    //사운드 클립
    [SerializeField]
    AudioClip lobbySceneAudioClip;
    [SerializeField]
    AudioClip gameSceneAuduiClip;
    [SerializeField]
    AudioClip stroySceneAudioClip;



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
                //Clip교체
                audioSource.clip = gameSceneAuduiClip;
                //재생
                audioSource.Play();
                break;
            //스토리씬
            case 3:
                break;
        }
    }
}
