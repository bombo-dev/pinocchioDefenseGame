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
        DoubleSpeedOptionButton, //��� �ɼ�,
        PlayOptionButton,    //��� �ɼ�
        RangeButton //��Ÿ� �ɼ�
    }

    enum TextMeshProUGUIs
    {
        DoubleSpeedOptionText,  //��� �ɼ�
        PlayOptionText, //��� �ɼ�
        StopOptionText,  //���� �ɼ�
        RangeText   //��Ÿ� �ؽ�Ʈ
    }

    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));

        //Ÿ�ӽ����� �ʱ�ȭ
        GetTextMeshProUGUI((int)TextMeshProUGUIs.DoubleSpeedOptionText).text = "X1.0";    //�ؽ�Ʈ ��ü
        GetTextMeshProUGUI((int)TextMeshProUGUIs.StopOptionText).gameObject.SetActive(true);    //�ؽ�Ʈ Ȱ��ȭ
        GetTextMeshProUGUI((int)TextMeshProUGUIs.PlayOptionText).gameObject.SetActive(false);    //�ؽ�Ʈ ��Ȱ��ȭ

        Time.timeScale = 1.0f;  //Ÿ�ӽ����� ����
        currentTimeScale = 1.0f;

        //��� �ɼ� �̺�Ʈ
        AddUIEvent(GetButton((int)Buttons.DoubleSpeedOptionButton).gameObject, OnClickDoubleSpeedButton, Define.UIEvent.Click);
        //����/���� �̺�Ʈ
        AddUIEvent(GetButton((int)Buttons.PlayOptionButton).gameObject, OnClickPlayOptionButton, Define.UIEvent.Click);
        //��Ÿ� ǥ�� �̺�Ʈ
        AddUIEvent(GetButton((int)Buttons.RangeButton).gameObject, OnClickRangeButton, Define.UIEvent.Click);
        UpdageRange();

        //Ʃ�丮�� ��ư ��Ȱ��ȭ ǥ�� (�� ����)
        if (SystemManager.Instance.GameFlowManager.stage == 0)
        {
            GetButton((int)Buttons.DoubleSpeedOptionButton).gameObject.GetComponent<Image>().color = Color.gray;
            GetButton((int)Buttons.PlayOptionButton).gameObject.GetComponent<Image>().color = Color.gray;
        }
    }

    /// <summary>
    /// ���� ��� ������ ���� Ÿ�� �������� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    public void OnClickDoubleSpeedButton(PointerEventData data)
    {
        //Ʃ�丮��
        if (SystemManager.Instance.GameFlowManager.stage == 0)
            return;

        //Ÿ�ӽ����� �׽�Ʈ

        if (Time.timeScale == 1.0f)
        {
            //�ؽ�Ʈ ��ü
            GetTextMeshProUGUI((int)TextMeshProUGUIs.DoubleSpeedOptionText).text = "X1.2";   

            //1.2������� ����
            Time.timeScale = 1.2f;
            currentTimeScale = 1.2f;
        }
        else if (Time.timeScale == 1.2f)
        {
            //�ؽ�Ʈ ��ü
            GetTextMeshProUGUI((int)TextMeshProUGUIs.DoubleSpeedOptionText).text = "X1.5";    //�ؽ�Ʈ ��ü

            //1.5������� ����
            Time.timeScale = 1.5f;
            currentTimeScale = 1.5f;
        }
        else if (Time.timeScale == 1.5f)
        {
            //�ؽ�Ʈ ��ü
            GetTextMeshProUGUI((int)TextMeshProUGUIs.DoubleSpeedOptionText).text = "X2.0";

            //1.5������� ����
            Time.timeScale = 2.0f;
            currentTimeScale = 2.0f;
        }
        else
        {
            //�ؽ�Ʈ ��ü
            GetTextMeshProUGUI((int)TextMeshProUGUIs.DoubleSpeedOptionText).text = "X1.0";

            //1.5������� ����
            Time.timeScale = 1.0f;
            currentTimeScale = 1.0f;
        }

        //FixedDeltaTime����
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    /// <summary>
    /// ���� �����϶� �ٽ� ����ϰ�, ��� �����϶� �ٽ� ���� : ������
    /// </summary>
    /// <param name="data"></param>
    public void OnClickPlayOptionButton(PointerEventData data = null)
    {
        //Ʃ�丮��
        if (SystemManager.Instance.GameFlowManager.stage == 0)
            return;

        //���� ������ ���
        if (Time.timeScale == 0f)
        {
            GetTextMeshProUGUI((int)TextMeshProUGUIs.StopOptionText).gameObject.SetActive(true);    //�ؽ�Ʈ Ȱ��ȭ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PlayOptionText).gameObject.SetActive(false);    //�ؽ�Ʈ ��Ȱ��ȭ

            //InputEvent Ȱ��ȭ
            SystemManager.Instance.InputManager.enabled = true;
            //UIȰ��ȭ
            SystemManager.Instance.PanelManager.turretInfoPanel.gameObject.SetActive(true);
            SystemManager.Instance.PanelManager.turretMngPanel.gameObject.SetActive(true);
            //��ưȰ��ȭ
            GetButton((int)Buttons.DoubleSpeedOptionButton).gameObject.SetActive(true);

            //���
            Time.timeScale = currentTimeScale;
        }
        //���� ������ ���
        else
        {
            GetTextMeshProUGUI((int)TextMeshProUGUIs.StopOptionText).gameObject.SetActive(false);    //�ؽ�Ʈ ��Ȱ��ȭ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PlayOptionText).gameObject.SetActive(true);    //�ؽ�Ʈ Ȱ��ȭ

            //InputEvent ��Ȱ��ȭ
            SystemManager.Instance.InputManager.enabled = false;
            
            //UI��Ȱ��ȭ
            SystemManager.Instance.PanelManager.turretInfoPanel.gameObject.SetActive(false);
            SystemManager.Instance.PanelManager.turretMngPanel.gameObject.SetActive(false);

            //��ư��Ȱ��ȭ
            GetButton((int)Buttons.DoubleSpeedOptionButton).gameObject.SetActive(false);

            //����
            Time.timeScale = 0;

        }

        //FixedDeltaTime����
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    /// <summary>
    /// ��Ÿ� �ѱ�,����: ������
    /// </summary>
    void OnClickRangeButton(PointerEventData data)
    {
        //��Ÿ� ���� ������ ���
        if (SystemManager.Instance.UserInfo.isShowRange)
        {
            //��Ÿ� ����
            SystemManager.Instance.UserInfo.isShowRange = false;
            if (SystemManager.Instance.RangeManager.rangeParents.childCount > 0)
                SystemManager.Instance.RangeManager.rangeParents.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

            GetTextMeshProUGUI((int)TextMeshProUGUIs.RangeText).text = "��Ÿ�\n�ѱ�";
        } 
        //��Ÿ� ���� ������ ���
        else 
        {
            SystemManager.Instance.UserInfo.isShowRange = true;
            if (SystemManager.Instance.RangeManager.rangeParents.childCount > 0)
                SystemManager.Instance.RangeManager.rangeParents.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

            GetTextMeshProUGUI((int)TextMeshProUGUIs.RangeText).text = "��Ÿ�\n����";
        }

        //���� ����
        SaveLoad Save = new SaveLoad();
        Save.SaveUserInfo();
    }

    /// <summary>
    /// ��Ÿ� �ɼǿ� ���� ��Ÿ�ǥ�� ���� ������Ʈ : ������
    /// </summary>
    void UpdageRange()
    {
        //��Ÿ� ���� ������ ���
        if (SystemManager.Instance.UserInfo.isShowRange)
        {
            //��Ÿ� �ѱ�
            if (SystemManager.Instance.RangeManager.rangeParents.childCount > 0)
                SystemManager.Instance.RangeManager.rangeParents.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

            GetTextMeshProUGUI((int)TextMeshProUGUIs.RangeText).text = "��Ÿ�\n����";
        }
        //��Ÿ� ���� ������ ���
        else
        {
            //��Ÿ� �ѱ�
            if (SystemManager.Instance.RangeManager.rangeParents.childCount > 0)
                SystemManager.Instance.RangeManager.rangeParents.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

            GetTextMeshProUGUI((int)TextMeshProUGUIs.RangeText).text = "��Ÿ�\n�ѱ�";
        }

        //���� ����
        SaveLoad Save = new SaveLoad();
        Save.SaveUserInfo();
    }

    /// <summary>
    /// �������� ����� ��Ȱ��ȭ �ؾ��ϴ� �г� ��Ȱ��ȭ : ������
    /// </summary>
    public void DisablePanelFinStage()
    {
        if (GetButton((int)Buttons.DoubleSpeedOptionButton).gameObject.activeSelf)
            GetButton((int)Buttons.DoubleSpeedOptionButton).gameObject.SetActive(false);
        if (GetButton((int)Buttons.PlayOptionButton).gameObject.activeSelf)
            GetButton((int)Buttons.PlayOptionButton).gameObject.SetActive(false);
    }
}
