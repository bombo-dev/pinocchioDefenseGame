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

    //����� �ҽ�
    public List<AudioSource> effectAudioSource;
    //����� �ҽ� �ε���
    int effectAudioSource_idx = 0;
    [SerializeField]
    Transform effectAudioTransform;

    public AudioClip buttonClickAudioClip;
    public AudioClip summonTurret;
    public AudioClip finConstruction;
    public AudioClip walk;

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
    public void ChangeEffectAudioClip(AudioClip audioClip)
    {
        //�ִ� �ε����� �ʱ�ȭ
        if (effectAudioSource.Count <= effectAudioSource_idx)
        {
            //�ε��� �ʱ�ȭ
            effectAudioSource_idx = 0;

            //�÷������̸� ������ҽ� ���� �� ����Ʈ�� �߰�
            if (effectAudioSource[effectAudioSource_idx].isPlaying)
            {
                //������Ʈ ����
                GameObject go = new GameObject("fireAudioSource");
                go.transform.parent = effectAudioTransform;

                //������Ʈ �߰�
                AudioSource goAs = go.AddComponent<AudioSource>();

                //����� �ҽ� ���� ����ȭ
                goAs.volume = SystemManager.Instance.UserInfo.efSoundVolume;

                if (SystemManager.Instance.UserInfo.isEfSound)
                    goAs.mute = false;
                else
                    goAs.mute = true;

                //����Ʈ�� �߰�
                effectAudioSource.Add(goAs);

                //�ε��� �� ������
                effectAudioSource_idx = effectAudioSource.Count - 1;
            }
        }

        //����� Ŭ�� ��ü �� ���
        effectAudioSource[effectAudioSource_idx].clip = audioClip;
        effectAudioSource[effectAudioSource_idx].Play();

        //�ε��� ����
        effectAudioSource_idx++;
    }

}
