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
    float woodResourceIncreaseCycle = 1.0f;

    //나무 자원 생성 주기마다 생성되는 나무 자원 수
    [SerializeField]
    int woodResourceIncreaseValue = 10;

    float timer;

    //User Info
    [Header("UserInfo")]
    //강화 나무 자원
    public int[] colorWoodResource; //0 - red, 1- yellow, 2 - green, 3 - white, 4 - blue, 5 - black

    //선택한 터렛 리스트
    public List<int> selectedTurretPreset;    //로비에서 넘겨온 터렛 프리셋

    private void Start()
    {
        //나무 자원 초기화
        if (SystemManager.Instance.GameFlowManager.stage > 0)
            woodResource = startWoodResource + ((SystemManager.Instance.GameFlowManager.stage - 1) * 50);
        else
            woodResource = 9999;

       //시간 측정용 변수 초기화
       timer = Time.time;


        //-------------------유저 자원 정보 불러오기-------------------
        UserInfo userInfo = SystemManager.Instance.UserInfo;
        //강화 나무자원
        colorWoodResource = new int[6];
        for (int i = 0; i < colorWoodResource.Length; i++)
            colorWoodResource[i] = userInfo.colorWoodResource[i];

        //터렛 프리셋 정보
        selectedTurretPreset.Clear();
        for (int i = 0; i < userInfo.turretPreset.Count; i++)
        {
            selectedTurretPreset.Add(userInfo.turretPreset[i]);
        }
    }

    private void Update()
    {
        if ((int)SystemManager.Instance.GameFlowManager.gameState == (int)GameFlowManager.GameState.Defense)
        {
            //일정 시간 마다 자동으로 쌓이는 나무자원, 
            IncreaseWoodResoruce_Auto();
        }
    }

    /// <summary>
    /// 일정 시간 마다 자동으로 나무자원 증가시키고 UI 정보에도 갱신 : 김현진
    /// </summary>
    void IncreaseWoodResoruce_Auto()
    {
        //woodResourceIncreaseCycle초마다 나무자원 갱신
        if (Time.time - timer > woodResourceIncreaseCycle)
        {
            int increaseWoodResource;

            if (SystemManager.Instance.UserInfo.selectMode == 0) // - 노말
                increaseWoodResource = woodResourceIncreaseValue + (SystemManager.Instance.UserInfo.selectedStageNum);
            else
                increaseWoodResource = woodResourceIncreaseValue + (SystemManager.Instance.UserInfo.selectedStageNum_hard);

            //나무 자원값 갱신
            woodResource += increaseWoodResource;

            // GoodsMngPanel 생성
            CreateGoodsPanel(increaseWoodResource, 1);

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
    public void IncreaseWoodResource(int increaseValue)
    {
        //나무 자원값 갱신
        woodResource += increaseValue;

        CreateGoodsPanel(increaseValue, 1);

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

        CreateGoodsPanel(decreaseValue, -1);

        //UI에도 적용
        if (SystemManager.Instance.PanelManager.resoursePanel)
        {
            UI_ResourcePanel resourcePanel = SystemManager.Instance.PanelManager.resoursePanel;
            resourcePanel.UpdateWoodResource();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">증가, 감소시킬 자원량</param>
    /// <param name="identity">증감 식별 변수: 양수면 자원 증가, 음수면 자원 감소</param>
    void CreateGoodsPanel(int value, int identity)
    {
        GameObject go = SystemManager.Instance.PanelManager.EnablePanel<RewardsMngPanel>(8);

        if (!go)
        {
            Debug.Log("CreateGoodsPanel Error!");
            return;
        }

        go.GetComponent<RewardsMngPanel>().ShowGold(value, identity);

    }
}
