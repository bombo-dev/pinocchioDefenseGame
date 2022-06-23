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
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));

        //�������� ���� ����
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageNumText).text = "Stage " +
            SystemManager.Instance.GameFlowManager.stage.ToString();

        GetTextMeshProUGUI((int)TextMeshProUGUIs.StageStartText).text = "Stage " +
           SystemManager.Instance.GameFlowManager.stage.ToString();

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
