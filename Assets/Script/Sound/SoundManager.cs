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
    [SerializeField]
    AudioSource audioSource;

    [Header("BG_Sound")]
    //���� Ŭ��
    [SerializeField]
    AudioClip lobbySceneAudioClip;
    [SerializeField]
    AudioClip gameSceneAuduiClip;
    [SerializeField]
    AudioClip stroySceneAudioClip;



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
                audioSource.clip = gameSceneAuduiClip;
                //���
                audioSource.Play();
                break;
            //���丮��
            case 3:
                break;
        }
    }
}
