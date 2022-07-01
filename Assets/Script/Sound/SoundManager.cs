using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager instance = null;
    //�̱��� ������Ƽ
    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }

    //����� �ҽ�
    public AudioSource audioSource;

    [Header("BG_Sound")]
    //���� Ŭ��
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
        //������ instance
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        //Scene�̵����� ������� �ʵ��� ó��
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeBGAudioClip(int sceneBuildIndex)
    {
        switch (sceneBuildIndex)
        {
            //�κ��
            //�ε���
            case 0:
            case 1:
                //Clip��ü
                audioSource.clip = lobbySceneAudioClip;
                //���
                audioSource.Play();
                break;
            //���Ӿ�
            case 2:
                //Clip��ü
                if(SystemManager.Instance.UserInfo.selectedStageNum <= 20)
                    audioSource.clip = gameSceneAuduiClip[0];
                else if (SystemManager.Instance.UserInfo.selectedStageNum <= 25)
                    audioSource.clip = gameSceneAuduiClip[1];
                else if (SystemManager.Instance.UserInfo.selectedStageNum <= 35)
                    audioSource.clip = gameSceneAuduiClip[2];
                else if (SystemManager.Instance.UserInfo.selectedStageNum <= 39)
                    audioSource.clip = gameSceneAuduiClip[3];
                else
                    audioSource.clip = gameSceneAuduiClip[4];
                //���
                audioSource.Play();
                break;
            //���丮��
            case 3:
                //Clip��ü
                audioSource.clip = stroySceneAudioClip;
                //���
                audioSource.Play();
                break;
            //�¸� 
            case 4:
                //Clip��ü
                audioSource.clip = winAudioClip;
                //���
                audioSource.Play();
                break;
            //�й�
            case 5:
                //Clip��ü
                audioSource.clip = failAudioClip;
                //���
                audioSource.Play();
                break;
        }
    }
}
