using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_OptionPanel : UI_Controller
{
    float currentTimeScale;

    DamageMngPanel damageMngPanel;

    enum Buttons
    {
        DoubleSpeedOptionButton, //배속 옵션,
        PlayOptionButton,    //재생 옵션
        RangeButton //사거리 옵션
    }

    enum TextMeshProUGUIs
    {
        DoubleSpeedOptionText,  //배속 옵션
        PlayOptionText, //재생 옵션
        StopOptionText,  //정지 옵션
        RangeText   //사거리 텍스트
    }

    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));

        //타임스케일 초기화
        GetTextMeshProUGUI((int)TextMeshProUGUIs.DoubleSpeedOptionText).text = "X1.0";    //텍스트 교체
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StopOptionText).gameObject.SetActive(true);    //텍스트 활성화
        GetTextMeshProUGUI((int)TextMeshProUGUIs.PlayOptionText).gameObject.SetActive(false);    //텍스트 비활성화

        Time.timeScale = 1.0f;  //타임스케일 변경
        currentTimeScale = 1.0f;

        //배속 옵션 이벤트
        AddUIEvent(GetButton((int)Buttons.DoubleSpeedOptionButton).gameObject, OnClickDoubleSpeedButton, Define.UIEvent.Click);
        //실행/정지 이벤트
        AddUIEvent(GetButton((int)Buttons.PlayOptionButton).gameObject, OnClickPlayOptionButton, Define.UIEvent.Click);
        //사거리 표시 이벤트
        AddUIEvent(GetButton((int)Buttons.RangeButton).gameObject, OnClickRangeButton, Define.UIEvent.Click);
        UpdageRange();

        //튜토리얼 버튼 비활성화 표시 (색 변경)
        if (SystemManager.Instance.GameFlowManager.stage == 0)
        {
            GetButton((int)Buttons.DoubleSpeedOptionButton).gameObject.GetComponent<Image>().color = Color.gray;
            GetButton((int)Buttons.PlayOptionButton).gameObject.GetComponent<Image>().color = Color.gray;
        }
    }

    /// <summary>
    /// 현재 배속 정보에 따라 타임 스케일을 변경 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    public void OnClickDoubleSpeedButton(PointerEventData data)
    {
        //튜토리얼
        if (SystemManager.Instance.GameFlowManager.stage == 0)
            return;

        //타임스케일 테스트

        if (Time.timeScale == 1.0f)
        {
            //텍스트 교체
            GetTextMeshProUGUI((int)TextMeshProUGUIs.DoubleSpeedOptionText).text = "X1.2";   

            //1.2배속으로 변경
            Time.timeScale = 1.2f;
            currentTimeScale = 1.2f;
        }
        else if (Time.timeScale == 1.2f)
        {
            //텍스트 교체
            GetTextMeshProUGUI((int)TextMeshProUGUIs.DoubleSpeedOptionText).text = "X1.5";    //텍스트 교체

            //1.5배속으로 변경
            Time.timeScale = 1.5f;
            currentTimeScale = 1.5f;
        }
        else if (Time.timeScale == 1.5f)
        {
            //텍스트 교체
            GetTextMeshProUGUI((int)TextMeshProUGUIs.DoubleSpeedOptionText).text = "X2.0";

            //1.5배속으로 변경
            Time.timeScale = 2.0f;
            currentTimeScale = 2.0f;
        }
        else
        {
            //텍스트 교체
            GetTextMeshProUGUI((int)TextMeshProUGUIs.DoubleSpeedOptionText).text = "X1.0";

            //1.5배속으로 변경
            Time.timeScale = 1.0f;
            currentTimeScale = 1.0f;
        }

        //FixedDeltaTime변경
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    /// <summary>
    /// 정지 상태일땐 다시 재생하고, 재생 상태일땐 다시 정지 : 김현진
    /// </summary>
    /// <param name="data"></param>
    public void OnClickPlayOptionButton(PointerEventData data = null)
    {
        //튜토리얼
        if (SystemManager.Instance.GameFlowManager.stage == 0)
            return;

        //정지 상태일 경우
        if (Time.timeScale == 0f)
        {
            GetTextMeshProUGUI((int)TextMeshProUGUIs.StopOptionText).gameObject.SetActive(true);    //텍스트 활성화
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PlayOptionText).gameObject.SetActive(false);    //텍스트 비활성화

            //InputEvent 활성화
            SystemManager.Instance.InputManager.enabled = true;
            //UI활성화
            SystemManager.Instance.PanelManager.turretInfoPanel.gameObject.SetActive(true);
            SystemManager.Instance.PanelManager.turretMngPanel.gameObject.SetActive(true);
            //버튼활성화
            GetButton((int)Buttons.DoubleSpeedOptionButton).gameObject.SetActive(true);

            //재생
            Time.timeScale = currentTimeScale;
        }
        //실행 상태일 경우
        else
        {
            GetTextMeshProUGUI((int)TextMeshProUGUIs.StopOptionText).gameObject.SetActive(false);    //텍스트 비활성화
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PlayOptionText).gameObject.SetActive(true);    //텍스트 활성화

            //InputEvent 비활성화
            SystemManager.Instance.InputManager.enabled = false;
            
            //UI비활성화
            SystemManager.Instance.PanelManager.turretInfoPanel.gameObject.SetActive(false);
            SystemManager.Instance.PanelManager.turretMngPanel.gameObject.SetActive(false);

            //버튼비활성화
            GetButton((int)Buttons.DoubleSpeedOptionButton).gameObject.SetActive(false);

            //정지
            Time.timeScale = 0;

        }

        //FixedDeltaTime변경
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    /// <summary>
    /// 사거리 켜기,끄기: 김현진
    /// </summary>
    void OnClickRangeButton(PointerEventData data)
    {
        //사거리 켜진 상태일 경우
        if (SystemManager.Instance.UserInfo.isShowRange)
        {
            //사거리 끄기
            SystemManager.Instance.UserInfo.isShowRange = false;
            if (SystemManager.Instance.RangeManager.rangeParents.childCount > 0)
                SystemManager.Instance.RangeManager.rangeParents.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

            GetTextMeshProUGUI((int)TextMeshProUGUIs.RangeText).text = "사거리\n켜기";
        } 
        //사거리 꺼진 상태일 경우
        else 
        {
            SystemManager.Instance.UserInfo.isShowRange = true;
            if (SystemManager.Instance.RangeManager.rangeParents.childCount > 0)
                SystemManager.Instance.RangeManager.rangeParents.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

            GetTextMeshProUGUI((int)TextMeshProUGUIs.RangeText).text = "사거리\n끄기";
        }

        //정보 저장
        SaveLoad Save = new SaveLoad();
        Save.SaveUserInfo();
    }

    /// <summary>
    /// 사거리 옵션에 따라 사거리표시 정보 업데이트 : 김현진
    /// </summary>
    void UpdageRange()
    {
        //사거리 켜진 상태일 경우
        if (SystemManager.Instance.UserInfo.isShowRange)
        {
            //사거리 켜기
            if (SystemManager.Instance.RangeManager.rangeParents.childCount > 0)
                SystemManager.Instance.RangeManager.rangeParents.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

            GetTextMeshProUGUI((int)TextMeshProUGUIs.RangeText).text = "사거리\n끄기";
        }
        //사거리 꺼진 상태일 경우
        else
        {
            //사거리 켜기
            if (SystemManager.Instance.RangeManager.rangeParents.childCount > 0)
                SystemManager.Instance.RangeManager.rangeParents.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

            GetTextMeshProUGUI((int)TextMeshProUGUIs.RangeText).text = "사거리\n켜기";
        }

        //정보 저장
        SaveLoad Save = new SaveLoad();
        Save.SaveUserInfo();
    }

    /// <summary>
    /// 스테이지 종료시 비활성화 해야하는 패널 비활성화 : 김현진
    /// </summary>
    public void DisablePanelFinStage()
    {
        if (GetButton((int)Buttons.DoubleSpeedOptionButton).gameObject.activeSelf)
            GetButton((int)Buttons.DoubleSpeedOptionButton).gameObject.SetActive(false);
        if (GetButton((int)Buttons.PlayOptionButton).gameObject.activeSelf)
            GetButton((int)Buttons.PlayOptionButton).gameObject.SetActive(false);
    }
}
