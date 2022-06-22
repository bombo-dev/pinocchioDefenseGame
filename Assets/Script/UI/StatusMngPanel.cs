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

    public int randPos;    // 랜덤으로 더해질 y축 값

    public float hp;

    Coroutine runningCoroutine = null;

    int i = 1;

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

    /// <summary>
    /// 디버프 설정
    /// </summary>
    /// <param name="debuffIdx"></param>
    /// <param name="debuffs"></param>
    /// <param name="time"></param>
    public void SetDebuff(int debuffIdx, Dictionary<Actor.debuff, Debuff> debuffs , float time)
    {        
        Debug.Log("-----------------------------------------SetDebuff "+i++);        
        // 예외처리
        if (debuffIdx <= 0 || hp <= 0)
            return;

        if (gameObject.activeSelf == false)
            return;

        // 해당 인덱스의 디버프 가져오기
        GameObject go = Debuffs[debuffIdx - 1];

        if (!go)
            return;

        // 디버프 텍스트 가져오기
        TextMeshProUGUI debuffText = go.GetComponentInChildren<TextMeshProUGUI>();     

        // 디버프 게이지 오브젝트 가져오기
        Transform GoTransform = go.transform.GetChild(0).transform.GetChild(0);
        Image ImgFillAmount = GoTransform.GetComponent<Image>();

        // 디버프의 경과시간을 카운트 할 변수 초기화
        debuffFlowTime = 0.0f;

        int stack = debuffs[(Actor.debuff)debuffIdx].stack;

        // 디버프 중첩시
        if (stack >= 2)
        {
            Debug.Log(stack + "중첩");

            // 이전에 실행되던 코루틴 종료
            if(runningCoroutine != null)
                StopCoroutine(runningCoroutine);

            // 중첩 정보를 화면에 표시
            debuffText.text = "X" + stack.ToString();
        }
        else
        {
            // 디버프 중첩 텍스트를 공백으로 표시
            debuffText.text = " ";
        }

        // 디버프 UI 활성화
        go.SetActive(true);

        // 코루틴 시작
        runningCoroutine = StartCoroutine(DebuffCoroutine(ImgFillAmount, time, debuffIdx));
    }
    
    /// <summary>
    /// 디버프 UI 제어를 위한 코루틴
    /// </summary>
    /// <param name="image">디버프 게이지를 표시할 이미지</param>
    /// <param name="time">디버프 지속시간</param>
    /// <param name="debuffIdx">디버프 인덱스</param>
    /// <returns></returns>
    IEnumerator DebuffCoroutine(Image image, float time, int debuffIdx)
    {
        while (true)
        {
            // 지속시간이 다됐을 때 or 패널이 비활성화 상태일 때
            if (debuffFlowTime >= time || gameObject.activeSelf == false)
            {                   
                // 코루틴 종료
                StopCoroutine(runningCoroutine);               

                // 디버프 패널 비활성화
                RemoveDebuff(debuffIdx);

            }
            // 디버프 경과시간 카운트
            debuffFlowTime += Time.deltaTime;
            Debug.Log("debuffFlowTime=" + debuffFlowTime);

            // 디버프 경과시간을 게이지 UI로 표시
            image.fillAmount = (debuffFlowTime/time);

            yield return new WaitForSeconds(Time.deltaTime);
                 
        }
    }
    
    public void RemoveDebuff(int debuffIndex)
    {
        
        GameObject go = Debuffs[debuffIndex-1];
        go.SetActive(false);
        
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
