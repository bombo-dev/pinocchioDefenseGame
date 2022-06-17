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
        PlayOptionButton    //재생 옵션
    }

    enum TextMeshProUGUIs
    {
        DoubleSpeedOptionText,  //배속 옵션
        PlayOptionText, //재생 옵션
        StopOptionText  //정지 옵션
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
    }

    /// <summary>
    /// 현재 배속 정보에 따라 타임 스케일을 변경 : 김현진
    /// </summary>
    /// <param name="data">이벤트 정보</param>
    public void OnClickDoubleSpeedButton(PointerEventData data)
    {
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
    public void OnClickPlayOptionButton(PointerEventData data)
    {
        //정지 상태일 경우
        if (Time.timeScale == 0f)
        {
            GetTextMeshProUGUI((int)TextMeshProUGUIs.StopOptionText).gameObject.SetActive(true);    //텍스트 활성화
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PlayOptionText).gameObject.SetActive(false);    //텍스트 비활성화

            //재생
            Time.timeScale = currentTimeScale;
        }
        //실행 상태일 경우
        else
        {
            GetTextMeshProUGUI((int)TextMeshProUGUIs.StopOptionText).gameObject.SetActive(false);    //텍스트 활성화
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PlayOptionText).gameObject.SetActive(true);    //텍스트 비활성화

            //정지
            Time.timeScale = 0;

            int panelNum = SystemManager.Instance.PanelManager.damagePanels.Count;
            Debug.Log("panelNum=" + panelNum);
            int i = 0;           

            while (i < panelNum)
            {
                damageMngPanel = SystemManager.Instance.PanelManager.damagePanels[0].GetComponent<DamageMngPanel>();
                //damageMngPanel.gameObject.SetActive(false);
                damageMngPanel.DisableDmgPanel(null, 0);
                i++;
            }            

        }

        //FixedDeltaTime변경
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }
}
