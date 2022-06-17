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

    [SerializeField]
    GameObject[] Debuffs;

    [SerializeField]
    Image Fill;

    float debuffFlowTime = 0.0f;

    float debuffLeftTime;

    public int randPos;    // 랜덤으로 더해질 y축 값

    public float hp;

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


    public void SetDebuff(int debuffIdx, Dictionary<Actor.debuff, Debuff> debuffs , float time)
    {
        if (debuffIdx <= 0 || hp <= 0)
            return;

        if (gameObject.activeSelf == false)
            return;

        GameObject go = Debuffs[debuffIdx - 1];

        if (!go)
            return;

        TextMeshProUGUI debuffText = go.GetComponentInChildren<TextMeshProUGUI>();

        int stack = debuffs[(Actor.debuff)debuffIdx].stack;
        

        Transform GoTransform = go.transform.GetChild(0).transform.GetChild(0);
        Image ImgFillAmount = GoTransform.GetComponent<Image>();

        debuffFlowTime = 0.0f;

        if (stack >= 2)
        {
            StopCoroutine(DebuffCoroutine(ImgFillAmount, time, debuffIdx));
            debuffText.text = "X" + stack.ToString();
            Debug.Log("잔여 시간=" + debuffLeftTime);
            time += debuffLeftTime;
            Debug.Log("합산 시간=" + time);
        }
        else
        {
            debuffText.text = " ";
            debuffLeftTime = 0.0f;
        }



        go.SetActive(true);

        StartCoroutine(DebuffCoroutine(ImgFillAmount, time, debuffIdx));

    }
    
    IEnumerator DebuffCoroutine(Image image, float time, int debuffIdx)
    {
        while (true)
        {

            if (debuffFlowTime >= time || gameObject.activeSelf == false)
            {
                StopCoroutine(DebuffCoroutine(image, time, debuffIdx));
                Debug.Log("Dead---------");
                RemoveDebuff(image, time, debuffIdx);
                //debuffFlowTime = 0.0f;

            }
            Debug.Log("time=" + time);
            debuffFlowTime += Time.deltaTime;
            image.fillAmount = (debuffFlowTime/time);
            debuffLeftTime = time - debuffFlowTime;
            Debug.Log("flowTime=" + debuffFlowTime);
            //Debug.Log("leftTime=" + debuffFlowTime);
            yield return new WaitForSeconds(Time.deltaTime);
                 
        }
    }
    
    public void RemoveDebuff(Image image, float time, int debuffIndex)
    {
        
        GameObject go = Debuffs[debuffIndex-1];
        go.SetActive(false);
        debuffLeftTime = 0.0f;
        
    }

    public void StatusReset()
    {
        // HPbar 정보 Reset
        GetSlider((int)Sliders.HPBar).value = 1.0f/1.0f;
        Fill.color = Color.red;

        //디버프 정보 Reset
        for(int i=0; i<Debuffs.Length; i++)
        {
            Debuffs[i].SetActive(false);
            Debuffs[i].GetComponentInChildren<TextMeshProUGUI>().text = null;
        }

    }

}
