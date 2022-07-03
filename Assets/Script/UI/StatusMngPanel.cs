using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StatusMngPanel : UI_Controller
{   

    public int enemyHPBarIndex;  // 활성화된 에너미 패널의 배열 인덱스

    public int turretHPBarIndex; // 활성화된 터렛 패널의 배열 인덱스

    public string filePath;

    public Vector3 panelPos;

    public GameObject hpBarOwner;   // hpBar를 갖고있는 유닛

    public Sprite greenBar;

    public Sprite redBar;

    [SerializeField]
    GameObject[] Debuffs;

    [SerializeField]
    Image Fill;

    float debuffFlowTime = 0.0f;

    public int randPos;    // 랜덤으로 더해질 y축 값

    public float hp;

    Coroutine runningCoroutine = null;

    int i = 1;

    bool SetTimeFlow = false;

    Image image;

    float durationTime;

    int debuffIndex;

    enum Images
    {
        Debuff_1_img,
        Debuff_2_img,
        Debuff_3_img,
        Debuff_4_img,
        Debuff_5_img,
        Debuff_6_img
    }

    enum Texts
    {
        Text_1,
        Text_2,
        Text_3,
        Text_4,
        Text_5,
        Text_6
    }
    public enum Sliders
    {
        HPBar
    }

    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Image>(typeof(Images));
        Bind<TextMesh>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));


    }

    private void Update()
    {
        //if (SetTimeFlow == true)
        //    UpdateDurationTime();
    }

    public void SetHPBarColor()
    {
        //Fill.color = Color.blue; //파랑
        //Fill.color = new Color(255 / 255f, 121 / 255f, 0 / 255f); //주황
        Fill.sprite = greenBar;
        Fill.color = Color.green;
    }

    public void SetHPBar(float currentHP, float maxHP)
    {
        if (currentHP <= 0)
            return;

        if (currentHP > maxHP)
            currentHP = maxHP;

        currentHP /= maxHP;

        GetSlider((int)Sliders.HPBar).value = currentHP;

        hp = currentHP;
    }

    /// <summary>
    /// 디버프 설정
    /// </summary>
    /// <param name="debuffIdx"></param>
    /// <param name="debuffs"></param>
    /// <param name="time"></param>
    public void SetDebuff(int debuffIdx, Dictionary<Actor.debuff, Debuff> debuffs , float time)
    {        
        //Debug.Log("-----------------------------------------SetDebuff "+i++);        
        // 예외처리
        if (debuffIdx <= 0 || hp <= 0)
            return;

        if (gameObject.activeSelf == false)
            return;


        // 해당 인덱스의 디버프 가져오기
        GameObject go = Debuffs[debuffIdx - 1];

        if (!go)
            return;

        SetDebuffMng setDebuffMng = go.GetComponent<SetDebuffMng>();

        // 디버프 화면에 띄우기
        setDebuffMng.ShowDebuff(debuffIdx, debuffs, time);

    }

    public void StatusReset()
    {
        // HPbar 정보 Reset
        GetSlider((int)Sliders.HPBar).value = 1.0f/1.0f;
        Fill.sprite = redBar;
        Fill.color = Color.red;

        //디버프 정보 Reset
        
        for(int i=0; i<Debuffs.Length; i++)
        {
            Debuffs[i].SetActive(false);
            Debuffs[i].GetComponentInChildren<TextMeshProUGUI>().text = null;
        }
        

    }

}
