using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    static SoundEffectManager instance = null;
    //싱글톤 프로퍼티
    public static SoundEffectManager Instance
    {
        get
        {
            return instance;
        }
    }

    //오디오 소스
    public List<AudioSource> effectAudioSource;
    //오디오 소스 인덱스
    int effectAudioSource_idx = 0;
    [SerializeField]
    Transform effectAudioTransform;

    public AudioClip buttonClickAudioClip;
    public AudioClip summonTurret;
    public AudioClip finConstruction;
    public AudioClip walk;

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

    /// <summary>
    /// FireAudio클립을 교체하고 재생 : 김현진
    /// </summary>
    /// <param name="audioClip">교체할 클립</param>
    public void ChangeEffectAudioClip(AudioClip audioClip)
    {
        //최대 인덱스면 초기화
        if (effectAudioSource.Count <= effectAudioSource_idx)
        {
            //인덱스 초기화
            effectAudioSource_idx = 0;

            //플레이중이면 오디오소스 생성 후 리스트에 추가
            if (effectAudioSource[effectAudioSource_idx].isPlaying)
            {
                //오브젝트 생성
                GameObject go = new GameObject("fireAudioSource");
                go.transform.parent = effectAudioTransform;

                //컴포넌트 추가
                AudioSource goAs = go.AddComponent<AudioSource>();

                //오디오 소스 정보 동기화
                goAs.volume = SystemManager.Instance.UserInfo.efSoundVolume;

                if (SystemManager.Instance.UserInfo.isEfSound)
                    goAs.mute = false;
                else
                    goAs.mute = true;

                //리스트에 추가
                effectAudioSource.Add(goAs);

                //인덱스 맨 끝으로
                effectAudioSource_idx = effectAudioSource.Count - 1;
            }
        }

        //오디오 클립 교체 후 재생
        effectAudioSource[effectAudioSource_idx].clip = audioClip;
        effectAudioSource[effectAudioSource_idx].Play();

        //인덱스 증가
        effectAudioSource_idx++;
    }

}
