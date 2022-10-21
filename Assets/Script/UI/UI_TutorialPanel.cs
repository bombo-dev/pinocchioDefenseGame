using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;

public class UI_TutorialPanel : UI_Controller
{
    Vector3 nestTourchPos = new Vector3(1.6f, 0, 0.18f);

    TurretManager turretManager;

    enum Step
    {
        welcom1,  //0
        welcom2,  //1
        welcom3,  //2
        touchNest, //3
        summonTurret,//4
        upgradeTurret,//5
        upgradeInfo,//6
        startDefense//7
    }
    Step step = Step.welcom1;

    enum TextMeshProUGUIs
    {
        TurretSummonText, //터렛 소환 가이드
        TurretUpgradeText, //터렛 강화 가이드
        NestTouchText, //터렛 터치 가이드
        WelcomeText,// 환영 멘트
        ImpossibleTurretRemoveText//터렛 철거 불가 경고
    }

    enum Images
    {
        TutorialPanel   //튜토리얼 패널 전체 이미지
    }

    enum Buttons
    {
        WelcomeTextButton,   //환영 멘트 넘기기 버튼
        WelcomeTextButton2
    }

    enum GameObjects
    {
        ColorWoodInfoPanel  //나무 강화 설명 패널
    }


    private void Update()
    {
        if (step == Step.touchNest)
        {
            UpdatePosNestTouchText();
            ChkClickNest();
        }
        if (step == Step.summonTurret)
        {
            ChkSummonTurret();
        }
        if (step == Step.upgradeTurret)
        {
            ChkUpgradeTurret();
        }
    }

    /// <summary>
    /// 둥지를 선택했을 경우 다음 튜토리얼으로 이동 : 김현진
    /// </summary>
    void ChkClickNest()
    {
        if (SystemManager.Instance.InputManager.currenstSelectNest)
        {
            //튜토리얼 텍스트 교체
            GetTextMeshProUGUI((int)TextMeshProUGUIs.NestTouchText).gameObject.SetActive(false);
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretSummonText).gameObject.SetActive(true);
            GetTextMeshProUGUI((int)TextMeshProUGUIs.ImpossibleTurretRemoveText).gameObject.SetActive(true);

            //나무 자원값 갱신
            SystemManager.Instance.ResourceManager.woodResource += 50;

            //UI에도 적용
            if (SystemManager.Instance.PanelManager.resoursePanel)
            {
                UI_ResourcePanel resourcePanel = SystemManager.Instance.PanelManager.resoursePanel;
                resourcePanel.UpdateWoodResource();
            }

            //강조 UI 맨 앞으로
            GetComponent<Image>().enabled = true;
            SystemManager.Instance.PanelManager.turretMngPanel.gameObject.transform.SetAsLastSibling();

            //상태 변경
            step = Step.summonTurret;
        }
    }

    /// <summary>
    /// 터렛을 소환 했을 경우 다음 튜토리얼으로 이동 : 김현진
    /// </summary>
    void ChkSummonTurret()
    {
        if (SystemManager.Instance.TurretManager.turrets.Count > 1)
        {
            //튜토리얼 텍스트 교체
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretSummonText).gameObject.SetActive(false);
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretUpgradeText).gameObject.SetActive(true);

            //상태 변경
            step = Step.upgradeTurret;
        }
    }

    /// <summary>
    /// 터렛을 업그레이드 했을 경우 다음 튜토리얼으로 이동 : 김현진
    /// </summary>
    void ChkUpgradeTurret()
    {
        if (SystemManager.Instance.TurretManager.turrets[1].GetComponent<Turret>().buffs.Count > 0)
        {
            //튜토리얼 텍스트 교체
            GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretUpgradeText).gameObject.SetActive(false);
            GetTextMeshProUGUI((int)TextMeshProUGUIs.ImpossibleTurretRemoveText).gameObject.SetActive(false);

            GetGameobject((int)GameObjects.ColorWoodInfoPanel).SetActive(true);

            GetButton((int)Buttons.WelcomeTextButton2).gameObject.SetActive(true);

            //상태변경
            step = Step.upgradeInfo;
        }
    }

    /// <summary>
    /// 터렛 클릭 텍스트 위치 업데이트 : 김현진
    /// </summary>
    void UpdatePosNestTouchText()
    {
        if (GetTextMeshProUGUI((int)TextMeshProUGUIs.NestTouchText).gameObject.activeSelf)
        {
            Vector3 vec = new Vector3(turretManager.tutorialNest.position.x, turretManager.tutorialNest.position.y + 30f, turretManager.tutorialNest.position.z);
            GetTextMeshProUGUI((int)TextMeshProUGUIs.NestTouchText).gameObject.transform.position =
                Camera.main.WorldToScreenPoint(vec);
        }
    }

    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        turretManager = SystemManager.Instance.TurretManager;

        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        //버튼 이벤트 추가
        AddUIEvent(GetButton((int)Buttons.WelcomeTextButton).gameObject, OnClickNext, Define.UIEvent.Click);
        AddUIEvent(GetButton((int)Buttons.WelcomeTextButton2).gameObject, OnClickNext, Define.UIEvent.Click);

        ///비활성화
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretSummonText).gameObject.SetActive(false);
        GetTextMeshProUGUI((int)TextMeshProUGUIs.TurretUpgradeText).gameObject.SetActive(false);
        GetTextMeshProUGUI((int)TextMeshProUGUIs.NestTouchText).gameObject.SetActive(false);
        GetTextMeshProUGUI((int)TextMeshProUGUIs.ImpossibleTurretRemoveText).gameObject.SetActive(false);

        GetButton((int)Buttons.WelcomeTextButton).gameObject.SetActive(false);
        GetButton((int)Buttons.WelcomeTextButton2).gameObject.SetActive(false);

        GetGameobject((int)GameObjects.ColorWoodInfoPanel).SetActive(false);

        //텍스트 초기화
        StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText), "투기장  \"케이지\" 에 오신것을 환영합니다", 0.05f));

    }

    /// <summary>
    /// 환영멘트 넘기기 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    public void OnClickNext(PointerEventData data)
    {
        //텍스트 변경
        if (step == Step.welcom1)
        {
            //텍스트 초기화
            StopCoroutine("Typing");
            StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText),
                "거점을 지키기 위해 몰려오는 적을 \n모두 섬멸하면 승리하시게 됩니다!", 0.05f));

            step = Step.welcom2;
        }
        else if (step == Step.welcom2)
        {
            //텍스트 초기화
            StopCoroutine("Typing");
            StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText),
                "터렛을 건설하여 승리를 쟁취하세요!", 0.05f));

            step = Step.welcom3;
        }
        else if (step == Step.welcom3)
        {
            //텍스트 활성화
            GetTextMeshProUGUI((int)TextMeshProUGUIs.NestTouchText).gameObject.SetActive(true);

            //이미지,텍스트 비활성화
            GetComponent<Image>().enabled = false;
            GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText).gameObject.SetActive(false);

            //터렛 관련 패널 활성화
            PanelManager pm = SystemManager.Instance.PanelManager;

            //터렛 소환 패널
            pm.EnablePanel<UI_TurretMngPanel>(0);

            //소환되어있는 터렛 정보 패널
            pm.EnablePanel<UI_TurretInfoPanel>(1);
            if (pm.turretInfoPanel)
                pm.turretInfoPanel.Reset();

            step = Step.touchNest;
        }
        else if (step == Step.upgradeInfo)
        {
            //텍스트 초기화
            GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText).gameObject.SetActive(true);

            //나무 자원값 갱신
            SystemManager.Instance.ResourceManager.woodResource += 100;

            StopCoroutine("Typing");
            StartCoroutine(Typing(GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText),
                "자, 그럼 전투 시작입니다!", 0.05f));

            GetGameobject((int)GameObjects.ColorWoodInfoPanel).SetActive(false);

            step = Step.startDefense;
        }
        else if (step == Step.startDefense)
        {
            GetComponent<Image>().enabled = false;
            GetTextMeshProUGUI((int)TextMeshProUGUIs.WelcomeText).gameObject.SetActive(false);

            //둥지 활성화
            for (int i = 0; i < SystemManager.Instance.BlockManager.tutorialNest.Length; i++)
                SystemManager.Instance.BlockManager.tutorialNest[i].SetActive(true);

            //디펜스 시작
            SystemManager.Instance.GameFlowManager.gameState = GameFlowManager.GameState.Defense;
        }


        //Next버튼 비활성화
        if (GetButton((int)Buttons.WelcomeTextButton).gameObject.activeSelf) 
            GetButton((int)Buttons.WelcomeTextButton).gameObject.SetActive(false);
        if (GetButton((int)Buttons.WelcomeTextButton2).gameObject.activeSelf)
            GetButton((int)Buttons.WelcomeTextButton2).gameObject.SetActive(false);
    }

    /// <summary>
    /// 타이핑 효과 : 김현진
    /// </summary>
    /// <param name="typingText">타이핑 효과를 줄 텍스트</param>
    /// <param name="message">텍스트 문장</param>
    /// <param name="speed">타이핑 속도</param>
    IEnumerator Typing(TextMeshProUGUI typingText, string message, float speed)
    {
        for (int i = 0; i < message.Length; i++)
        {
            typingText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }

        //Next버튼 활성화
        GetButton((int)Buttons.WelcomeTextButton).gameObject.SetActive(true);
    }
}
