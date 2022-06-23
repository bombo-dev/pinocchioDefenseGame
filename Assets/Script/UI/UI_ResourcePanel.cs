using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ResourcePanel : UI_Controller
{
    public string filePath;

    enum TextMeshProUGUIs
    {
        woodResourceText,
        StageNumText,
        StageStartText
    }

    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));

        //스테이지 정보 갱신
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageNumText).text = "Stage " +
            SystemManager.Instance.GameFlowManager.stage.ToString();

        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageStartText).text = "Stage " +
           SystemManager.Instance.GameFlowManager.stage.ToString();

        //나무 자원 UI 초기화
        UpdateWoodResource();
    }

    /// <summary>
    /// 현재 나무 자원값을 받아와 나무 자원을 표시해주는 UI를 갱신
    /// </summary>
    public void UpdateWoodResource()
    {
        //현재 나무 자원을 받아와 텍스트값을 변경
        GetTextMeshProUGUI((int)TextMeshProUGUIs.woodResourceText).text = SystemManager.Instance.ResourceManager.woodResource.ToString();
        SystemManager.Instance.PanelManager.turretMngPanel.UpdateWoodResource();
    }
}
