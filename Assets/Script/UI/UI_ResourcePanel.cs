using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ResourcePanel : UI_Controller
{
    public string filePath;

    [SerializeField]
    Animator animator;

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

    enum GameObjects
    {
        StageStartPanel
    }

    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        //�������� ���� ����
        if (SystemManager.Instance.GameFlowManager.stage == 0)
        {
            //Ʃ�丮�󿡼��� ��Ȱ��ȭ
            animator.enabled = false;

            //�г� ��Ȱ��ȭ
            GetGameobject((int)GameObjects.StageStartPanel).SetActive(false);
        }
        else
        {
            GetTextMeshProUGUI((int)TextMeshProUGUIs.StageNumText).text = "Stage " +
           SystemManager.Instance.GameFlowManager.stage.ToString();

            GetTextMeshProUGUI((int)TextMeshProUGUIs.StageStartText).text = "Stage " +
               SystemManager.Instance.GameFlowManager.stage.ToString();

            //�������� �̹��� ����
            if(SystemManager.Instance.GameFlowManager.stage > 0)
                if (((SystemManager.Instance.GameFlowManager.stage - 1) / 10) < stageSprite.Length)
                    GetImage((int)Images.StageStartImage).sprite = stageSprite[SystemManager.Instance.GameFlowManager.stage / 10];

            //���潺 ���� �ڷ�ƾ ȣ��
            StartCoroutine("StartDefense");
        }
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

    /// <summary>
    /// ���ۻ��¿��� ���潺 ���·� ���� : ������
    /// </summary>
    IEnumerator StartDefense()
    {
        yield return new WaitForSeconds(2.0f);
        //���潺 ����
        SystemManager.Instance.GameFlowManager.gameState = GameFlowManager.GameState.Defense;
        //UIȰ��ȭ
        SystemManager.Instance.PanelManager.EnableFixedPanel(2);
        //���� �ڿ� UI �ʱ�ȭ
        UpdateWoodResource();
    }

}
