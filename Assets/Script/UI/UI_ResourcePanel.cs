using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ResourcePanel : UI_Controller
{
    public string filePath;

    [SerializeField]
    Sprite[] stageSprite;

    enum TextMeshProUGUIs
    {
        woodResourceText,
        StageNumText,
        StageStartText
    }

    enum Images
    {
        StageStartImage
    }

    /// <summary>
    /// enum에 열거된 이름으로 UI정보를 바인딩 : 김현진
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<Image>(typeof(Images));

        //스테이지 정보 갱신
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageNumText).text = "Stage " +
            SystemManager.Instance.GameFlowManager.stage.ToString();

        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageStartText).text = "Stage " +
           SystemManager.Instance.GameFlowManager.stage.ToString();


        //스테이지 이미지 변경
        if((SystemManager.Instance.GameFlowManager.stage / 10 + 1) < stageSprite.Length)
            GetImage((int)Images.StageStartImage).sprite = stageSprite[SystemManager.Instance.GameFlowManager.stage / 10 + 1];

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
