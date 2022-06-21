using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    static SoundEffectManager instance = null;
    //�̱��� ������Ƽ
    public static SoundEffectManager Instance
    {
        get
        {
            return instance;
        }
    }

    //Fire ����� �ҽ�
    public AudioSource[] fireAudioSource;
    //Fire ����� �ҽ� �ε���
    int fireAudioSource_idx = 0;

    //Upgrade ����� �ҽ�
    public AudioSource[] upgradeAudioSource;
    //Upgrade ����� �ҽ� �ε���
    int upgradeAudioSource_idx = 0;

    //Damage ����� �ҽ�
    public AudioSource[] damageAudioSource;
    //Damage ����� �ҽ� �ε���
    int damageAudioSource_idx = 0;

    //Death ����� �ҽ�
    public AudioSource[] deathAudioSource;
    //Death ����� �ҽ� �ε���
    int deathAudioSource_idx = 0;

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

    /// <summary>
    /// FireAudioŬ���� ��ü�ϰ� ��� : ������
    /// </summary>
    /// <param name="audioClip">��ü�� Ŭ��</param>
    public void ChangefireAudioClip(AudioClip audioClip)
    {
        //�ִ� �ε����� �ʱ�ȭ
        if (fireAudioSource.Length <= fireAudioSource_idx)
            fireAudioSource_idx = 0;

        //����� Ŭ�� ��ü �� ���
        fireAudioSource[fireAudioSource_idx].clip = audioClip;
        fireAudioSource[fireAudioSource_idx].Play();

        //�ε��� ����
        fireAudioSource_idx++;
    }

    /// <summary>
    /// UpgradeAudioŬ���� ��ü�ϰ� ��� : ������
    /// </summary>
    /// <param name="audioClip">��ü�� Ŭ��</param>
    public void ChangeUpgradeAudioClip(AudioClip audioClip)
    {
        //�ִ� �ε����� �ʱ�ȭ
        if (upgradeAudioSource.Length <= upgradeAudioSource_idx)
            upgradeAudioSource_idx = 0;

        //����� Ŭ�� ��ü �� ���
        upgradeAudioSource[upgradeAudioSource_idx].clip = audioClip;
        upgradeAudioSource[upgradeAudioSource_idx].Play();

        //�ε��� ����
        upgradeAudioSource_idx++;
    }

    /// <summary>
    /// DamageAudioŬ���� ��ü�ϰ� ��� : ������
    /// </summary>
    /// <param name="audioClip">��ü�� Ŭ��</param>
    public void ChangeDamageAudioClip(AudioClip audioClip)
    {
        //�ִ� �ε����� �ʱ�ȭ
        if (damageAudioSource.Length <= damageAudioSource_idx)
            damageAudioSource_idx = 0;

        //����� Ŭ�� ��ü �� ���
        damageAudioSource[damageAudioSource_idx].clip = audioClip;
        damageAudioSource[damageAudioSource_idx].Play();

        //�ε��� ����
        damageAudioSource_idx++;
    }

    /// <summary>
    /// DeathAudioŬ���� ��ü�ϰ� ��� : ������
    /// </summary>
    /// <param name="audioClip">��ü�� Ŭ��</param>
    public void ChangeDeathAudioClip(AudioClip audioClip)
    {
        //�ִ� �ε����� �ʱ�ȭ
        if (deathAudioSource.Length <= deathAudioSource_idx)
            deathAudioSource_idx = 0;

        //����� Ŭ�� ��ü �� ���
        deathAudioSource[deathAudioSource_idx].clip = audioClip;
        deathAudioSource[deathAudioSource_idx].Play();

        //�ε��� ����
        deathAudioSource_idx++;
    }

}
