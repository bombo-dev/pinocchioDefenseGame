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
            if (SystemManager.Instance.UserInfo.selectedStageNum <= 10)
                    GetImage((int)Images.StageStartImage).sprite = stageSprite[0]; //�����
            else if(SystemManager.Instance.UserInfo.selectedStageNum <= 20)
                    GetImage((int)Images.StageStartImage).sprite = stageSprite[1]; //�ǹ�
            else if (SystemManager.Instance.UserInfo.selectedStageNum <= 25)
                GetImage((int)Images.StageStartImage).sprite = stageSprite[2]; //���
            else if (SystemManager.Instance.UserInfo.selectedStageNum <= 30)
                GetImage((int)Images.StageStartImage).sprite = stageSprite[3]; //�÷�Ƽ��
            else if (SystemManager.Instance.UserInfo.selectedStageNum <= 35)
                GetImage((int)Images.StageStartImage).sprite = stageSprite[4]; //���̾�
            else 
                GetImage((int)Images.StageStartImage).sprite = stageSprite[5]; //������

            //���� ��������
            if (SystemManager.Instance.UserInfo.selectedStageNum == 11 ||
                SystemManager.Instance.UserInfo.selectedStageNum == 20 ||
                SystemManager.Instance.UserInfo.selectedStageNum == 28 ||
                SystemManager.Instance.UserInfo.selectedStageNum == 36 ||
                SystemManager.Instance.UserInfo.selectedStageNum == 38 ||
                SystemManager.Instance.UserInfo.selectedStageNum == 40)
            {
                //���� ���潺 ���� �ڷ�ƾ ȣ��
                StartCoroutine("StartBossDefense");
            }
            //�Ϲ� ��������
            else
            {
                //���潺 ���� �ڷ�ƾ ȣ��
                StartCoroutine("StartDefense");
            }
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
        SystemManager.Instance.PanelManager.optionPopUpPanel.EnablePediaButton();
        //���� �ڿ� UI �ʱ�ȭ
        UpdateWoodResource();
    }

    /// <summary>
    /// ���ۻ��¿��� ���潺 ���·� ���� - ���� : ������
    /// </summary>
    IEnumerator StartBossDefense()
    {
        yield return new WaitForSeconds(2.0f);

        //UIȰ��ȭ
        SystemManager.Instance.PanelManager.EnablePanel<UI_BossPanel>(14);

        yield return new WaitForSeconds(7.0f);

        //���潺 ����
        SystemManager.Instance.GameFlowManager.gameState = GameFlowManager.GameState.Defense;

        //UIȰ��ȭ
        SystemManager.Instance.PanelManager.EnableFixedPanel(2);
        SystemManager.Instance.PanelManager.optionPopUpPanel.EnablePediaButton();

        //���� �ڿ� UI �ʱ�ȭ
        UpdateWoodResource();

        //Ÿ�̸� �ʱ�ȭ
        for (int i = 0; i < SystemManager.Instance.GameFlowManager.flowTimer.Length; i++)
        {
            SystemManager.Instance.GameFlowManager.flowTimer[i] = Time.time;
        }
    }

}
