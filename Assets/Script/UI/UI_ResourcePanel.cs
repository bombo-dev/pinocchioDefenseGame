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
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<Image>(typeof(Images));

        //�������� ���� ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageNumText).text = "Stage " +
            SystemManager.Instance.GameFlowManager.stage.ToString();

        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageStartText).text = "Stage " +
           SystemManager.Instance.GameFlowManager.stage.ToString();


        //�������� �̹��� ����
        if((SystemManager.Instance.GameFlowManager.stage / 10 + 1) < stageSprite.Length)
            GetImage((int)Images.StageStartImage).sprite = stageSprite[SystemManager.Instance.GameFlowManager.stage / 10 + 1];

        //���� �ڿ� UI �ʱ�ȭ
        UpdateWoodResource();
    }

    /// <summary>
    /// ���� ���� �ڿ����� �޾ƿ� ���� �ڿ��� ǥ�����ִ� UI�� ����
    /// </summary>
    public void UpdateWoodResource()
    {
        //���� ���� �ڿ��� �޾ƿ� �ؽ�Ʈ���� ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.woodResourceText).text = SystemManager.Instance.ResourceManager.woodResource.ToString();
        SystemManager.Instance.PanelManager.turretMngPanel.UpdateWoodResource();
    }
}
