using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    //터렛 생성 등에 쓰이는 나무 자원
    public int woodResource = 0;

    //나무 자원 초기값
    [SerializeField]
    int startWoodResource = 500;

    //나무 자원 생성 주기
    [SerializeField]
    float woodResourceIncreaseCycle = 2.0f;

    //나무 자원 생성 주기마다 생성되는 나무 자원 수
    [SerializeField]
    int woodResourceIncreaseValue = 10;

    //시간 측정용 변수
    float timer;

    private void Start()
    {
        //나무 자원 초기화
        woodResource = startWoodResource;

        //시간 측정용 변수 초기화
        timer = Time.time;
    }

    private void Update()
    {
        //일정 시간 마다 자동으로 쌓이는 나무자원
        IncreaseWoodResoruce_Auto();
    }

    /// <summary>
    /// 일정 시간 마다 자동으로 나무자원 증가시키고 UI 정보에도 갱신 : 김현진
    /// </summary>
    void IncreaseWoodResoruce_Auto()
    {
        //woodResourceIncreaseCycle초마다 나무자원 갱신
        if (Time.time - timer > woodResourceIncreaseCycle)
        {
            //나무 자원값 갱신
            woodResource += woodResourceIncreaseValue;

            //UI에도 적용
            if (SystemManager.Instance.PanelManager.resoursePanel)
            {
                UI_ResourcePanel resourcePanel = SystemManager.Instance.PanelManager.resoursePanel;
                resourcePanel.UpdateWoodResource();
            }

            //시간 측정용 변수 초기화
            timer = Time.time;
        }
    }

    /// <summary>
    /// 나무 자원 증가시키고 UI정보에도 갱신 : 김현진
    /// </summary>
    /// <param name="increaseValue">증가시킬 값</param>
    void IncreaseWoodResource(int increaseValue)
    {
        //나무 자원값 갱신
        woodResource += increaseValue;

        //UI에도 적용
        if (SystemManager.Instance.PanelManager.resoursePanel)
        {
            UI_ResourcePanel resourcePanel = SystemManager.Instance.PanelManager.resoursePanel;
            resourcePanel.UpdateWoodResource();
        }
    }

    /// <summary>
    /// 나무 자원 감소시키고 UI정보에도 갱신 : 김현진
    /// </summary>
    /// <param name="decreaseValue">감소시킬 값</param>
    public void DecreaseWoodResource(int decreaseValue)
    {
        //나무 자원값 갱신
        if (woodResource <= decreaseValue)
            woodResource = 0;
        else
            woodResource -= decreaseValue;

        //UI에도 적용
        if (SystemManager.Instance.PanelManager.resoursePanel)
        {
            UI_ResourcePanel resourcePanel = SystemManager.Instance.PanelManager.resoursePanel;
            resourcePanel.UpdateWoodResource();
        }
    }
}
