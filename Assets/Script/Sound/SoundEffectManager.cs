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

    //Fire 오디오 소스
    public AudioSource[] fireAudioSource;
    //Fire 오디오 소스 인덱스
    int fireAudioSource_idx = 0;

    //Upgrade 오디오 소스
    public AudioSource[] upgradeAudioSource;
    //Upgrade 오디오 소스 인덱스
    int upgradeAudioSource_idx = 0;

    //Damage 오디오 소스
    public AudioSource[] damageAudioSource;
    //Damage 오디오 소스 인덱스
    int damageAudioSource_idx = 0;

    //Death 오디오 소스
    public AudioSource[] deathAudioSource;
    //Death 오디오 소스 인덱스
    int deathAudioSource_idx = 0;

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
    public void ChangefireAudioClip(AudioClip audioClip)
    {
        //최대 인덱스면 초기화
        if (fireAudioSource.Length <= fireAudioSource_idx)
            fireAudioSource_idx = 0;

        //오디오 클립 교체 후 재생
        fireAudioSource[fireAudioSource_idx].clip = audioClip;
        fireAudioSource[fireAudioSource_idx].Play();

        //인덱스 증가
        fireAudioSource_idx++;
    }

    /// <summary>
    /// UpgradeAudio클립을 교체하고 재생 : 김현진
    /// </summary>
    /// <param name="audioClip">교체할 클립</param>
    public void ChangeUpgradeAudioClip(AudioClip audioClip)
    {
        //최대 인덱스면 초기화
        if (upgradeAudioSource.Length <= upgradeAudioSource_idx)
            upgradeAudioSource_idx = 0;

        //오디오 클립 교체 후 재생
        upgradeAudioSource[upgradeAudioSource_idx].clip = audioClip;
        upgradeAudioSource[upgradeAudioSource_idx].Play();

        //인덱스 증가
        upgradeAudioSource_idx++;
    }

    /// <summary>
    /// DamageAudio클립을 교체하고 재생 : 김현진
    /// </summary>
    /// <param name="audioClip">교체할 클립</param>
    public void ChangeDamageAudioClip(AudioClip audioClip)
    {
        //최대 인덱스면 초기화
        if (damageAudioSource.Length <= damageAudioSource_idx)
            damageAudioSource_idx = 0;

        //오디오 클립 교체 후 재생
        damageAudioSource[damageAudioSource_idx].clip = audioClip;
        damageAudioSource[damageAudioSource_idx].Play();

        //인덱스 증가
        damageAudioSource_idx++;
    }

    /// <summary>
    /// DeathAudio클립을 교체하고 재생 : 김현진
    /// </summary>
    /// <param name="audioClip">교체할 클립</param>
    public void ChangeDeathAudioClip(AudioClip audioClip)
    {
        //최대 인덱스면 초기화
        if (deathAudioSource.Length <= deathAudioSource_idx)
            deathAudioSource_idx = 0;

        //오디오 클립 교체 후 재생
        deathAudioSource[deathAudioSource_idx].clip = audioClip;
        deathAudioSource[deathAudioSource_idx].Play();

        //인덱스 증가
        deathAudioSource_idx++;
    }

}
