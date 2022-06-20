using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class UI_TurretMngPanel : UI_Controller
{
    public string filePath;

    const int MAXTURRET = 23;   //최대 터렛 수

    int currentSelectedTurretIdx = 0;   //현재 선택한 터렛 번호

    
    Actor actor; // HPBar 위치 업데이트를 위함

    Turret baseTurret;  //베이스 터렛

    enum TextMeshProUGUIs
    {
        TurretText0, //0~
        TurretText1,
        TurretText2,
        TurretText3,
        TurretText4,
        TurretText5,
        TurretText6,
        TurretText7,
        TurretText8,
        TurretText9,
        TurretText10,
        TurretText11,
        TurretText12,
        TurretText13,
        TurretText14,
        TurretText15,
        TurretText16,
        TurretText17,
        TurretText18,
        TurretText19,
        TurretText20,
        TurretText21,
        TurretText22, //~22
        ConstructionText0,
        ConstructionText1,
        ConstructionText2,
        ConstructionText3,
        ConstructionText4,
        ConstructionText5,
        ConstructionText6,
        ConstructionText7,
        ConstructionText8,
        ConstructionText9,
        ConstructionText10,
        ConstructionText11,
        ConstructionText12,
        ConstructionText13,
        ConstructionText14,
        ConstructionText15,
        ConstructionText16,
        ConstructionText17,
        ConstructionText18,
        ConstructionText19,
        ConstructionText20,
        ConstructionText21,
        ConstructionText22,
        WoodResourceText   //현재 보유하고있는 자원 표시
    }

    enum Buttons
    {
        TurretButton0, //0~
        TurretButton1,
        TurretButton2,
        TurretButton3,
        TurretButton4,
        TurretButton5,
        TurretButton6,
        TurretButton7,
        TurretButton8,
        TurretButton9,
        TurretButton10,
        TurretButton11,
        TurretButton12,
        TurretButton13,
        TurretButton14,
        TurretButton15,
        TurretButton16,
        TurretButton17,
        TurretButton18,
        TurretButton19,
        TurretButton20,
        TurretButton21,
        TurretButton22 //~22
    }

    enum GameObjects
    {
        TurretPanel0,   //0~
        TurretPanel1,
        TurretPanel2,
        TurretPanel3,
        TurretPanel4,
        TurretPanel5,
        TurretPanel6,
        TurretPanel7,
        TurretPanel8,
        TurretPanel9,
        TurretPanel10,
        TurretPanel11,
        TurretPanel12,
        TurretPanel13,
        TurretPanel14,
        TurretPanel15,
        TurretPanel16,
        TurretPanel17,
        TurretPanel18,
        TurretPanel19,
        TurretPanel20,
        TurretPanel21,
        TurretPanel22   //~22
    }

    public enum Sliders
    {
        BaseHP
    }

    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Slider>(typeof(Sliders));

        //인덱스 선언
        int idx = 0;
        bool endTurret = false;
        //탐색할 리스트 정렬
        SystemManager.Instance.ResourceManager.selectedTurretPreset.Sort();

        //터렛 선택 버튼 이벤트 추가
        for (int i = 0; i < MAXTURRET; i++) 
        {
            //선택된 터렛인경우 버튼 활성화
            if (!endTurret)
            {
                if (SystemManager.Instance.ResourceManager.selectedTurretPreset[idx] == i)
                {
                    AddUIEvent(GetButton(i).gameObject, i, OnClickTurretButton, Define.UIEvent.Click);

                    //터렛 패널 정보 초기화
                    ResetTurretInfo(idx);

                    idx++;

                    if (idx >= SystemManager.Instance.ResourceManager.selectedTurretPreset.Count)
                        endTurret = true;
                }
                else
                {
                    if (GetGameobject((int)GameObjects.TurretPanel0 + i).activeSelf)
                        GetGameobject((int)GameObjects.TurretPanel0 + i).SetActive(false);
                }
            }
            //선택된 터렛이 아닐경우 버튼 비활성화
            else
            {
                if (GetGameobject((int)GameObjects.TurretPanel0 + i).activeSelf)
                    GetGameobject((int)GameObjects.TurretPanel0 + i).SetActive(false);
            }
        }
    }

    /// <summary>
    /// 터렛매니저 UI의 터렛정보 리셋 : 김현진
    /// </summary>
    private void ResetTurretInfo(int idx)
    {
        //예외처리
        if (SystemManager.Instance.TurretManager.turretCostArr.Length < MAXTURRET)
            SystemManager.Instance.TurretManager.InitializeTurretArrData();

        // 터렛 코스트 정보 초기화
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretText0 + idx).text = SystemManager.Instance.TurretManager.turretCostArr[idx].ToString();

        //터렛 건설 시간 정보 초기화
        GetTextMeshProUGUI((int)TextMeshProUGUIs.ConstructionText0 + idx).text = "공사시간: " + SystemManager.Instance.TurretManager.turretConstructionTimeArr[idx].ToString() + "초";
    }

    /// <summary>
    /// 소환할 터렛을 선택하거나 더블 터치 or 클릭으로 소환
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    /// <param name="idx">소환할 터렛의 인덱스</param>
    public void OnClickTurretButton(PointerEventData data, int idx)
    {
        currentSelectedTurretIdx = idx;

        OnClickTurretSummonButton(data);
    }

    /// <summary>
    /// 선택해놓은 터렛을 소환 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    public void OnClickTurretSummonButton(PointerEventData data)
    {
        //건설 비용 지불
        int cost = int.Parse(GetTextMeshProUGUI(currentSelectedTurretIdx).text);

        if (cost <= 0)
            return;

        //비용이 부족한경우 건설불가
        if (cost > SystemManager.Instance.ResourceManager.woodResource)
        {
            //비용부족 처리
            return;
        }

        if (currentSelectedTurretIdx >= 0 && currentSelectedTurretIdx < MAXTURRET && SystemManager.Instance.InputManager.currenstSelectNest != null)
        {
            GameObject nestGo = SystemManager.Instance.InputManager.currenstSelectNest;

            //예외처리
            if (!nestGo)
                return;

            Nest nest = nestGo.GetComponent<Nest>();

            //예외처리
            if (!nest)
                return;

            //이미 터렛이 존재하거나 공사중일 경우
            if (nest.haveTurret || nest.construction)
                return;

            //공사용 터렛 소환
            GameObject turretGo = SystemManager.Instance.TurretManager.EnableTurret(SystemManager.Instance.TurretManager.CONSTRUCTIONTURRET_INDEX, nestGo.transform.position);

            //예외처리
            if (!turretGo)
                return;

            ConstructionTurret constructTurret = turretGo.GetComponent<ConstructionTurret>();

            //예외처리
            if (!constructTurret)
                return;

            //UI_ConstructionGaugePanel생성
            GameObject constructionGaugePanel = SystemManager.Instance.PanelManager.EnablePanel<UI_ConstructionGauge>(5, turretGo);

            //공사용 터렛에 주요 변수정보 넘겨주기
            constructTurret.timer = Time.time;  //타이머 초기화
            constructTurret.currentSelectedTurretIdx = currentSelectedTurretIdx;    //소환될 터렛 인덱스
            constructTurret.nestGo = nestGo;    //소환할 둥지    
            constructTurret.constructionValue = 0;  //건설 게이지 값 초기화
            constructTurret.constructionTime = SystemManager.Instance.TurretManager.turretConstructionTimeArr[currentSelectedTurretIdx]; //건설에 걸리는 시간 초기화
            constructTurret.constructionGaugePanel = constructionGaugePanel;    //건설 게이지 패널 정보

            //공사중정보 둥지에 전달
            nest.construction = true;
            nest.turret = turretGo;

            //터렛 공사시작
            constructTurret.startConstruction = true;   //공사시작

            //UI_TurretInfoPanel 패널이 존재할 경우 공사중 정보 갱신
            if (SystemManager.Instance.PanelManager.turretInfoPanel)
            {
                //패널 갱신
                SystemManager.Instance.PanelManager.turretInfoPanel.Reset();
            }

            //비용지불
            SystemManager.Instance.ResourceManager.DecreaseWoodResource(cost);

        }
    }

    /// <summary>
    /// 슬라이드바의 값을 실시간으로 갱신 해준다 : 김현진
    /// </summary>
    public void UpdateSlideBar()
    {
        if (SystemManager.Instance.TurretManager.baseTurret)
            GetSlider((int)Sliders.BaseHP).value =  (float)SystemManager.Instance.TurretManager.baseTurret.currentHP / (float)SystemManager.Instance.TurretManager.baseTurret.maxHP;
    }

    /// <summary>
    /// 현재 나무 자원값을 받아와 나무 자원을 표시해주는 UI를 갱신
    /// </summary>
    public void UpdateWoodResource()
    {
        //현재 나무 자원을 받아와 텍스트값을 변경
        GetTextMeshProUGUI((int)TextMeshProUGUIs.WoodResourceText).text = SystemManager.Instance.ResourceManager.woodResource.ToString();
    }

}
