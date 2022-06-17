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
        PlayOptionButton    //��� �ɼ�
    }

    enum TextMeshProUGUIs
    {
        DoubleSpeedOptionText,  //��� �ɼ�
        PlayOptionText, //��� �ɼ�
        StopOptionText  //���� �ɼ�
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
    }

    /// <summary>
    /// ���� ��� ������ ���� Ÿ�� �������� ���� : ������
    /// </summary>
    /// <param name="data">�̺�Ʈ ����</param>
    public void OnClickDoubleSpeedButton(PointerEventData data)
    {
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
    public void OnClickPlayOptionButton(PointerEventData data)
    {
        //���� ������ ���
        if (Time.timeScale == 0f)
        {
            GetTextMeshProUGUI((int)TextMeshProUGUIs.StopOptionText).gameObject.SetActive(true);    //�ؽ�Ʈ Ȱ��ȭ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PlayOptionText).gameObject.SetActive(false);    //�ؽ�Ʈ ��Ȱ��ȭ

            //���
            Time.timeScale = currentTimeScale;
        }
        //���� ������ ���
        else
        {
            GetTextMeshProUGUI((int)TextMeshProUGUIs.StopOptionText).gameObject.SetActive(false);    //�ؽ�Ʈ Ȱ��ȭ
            GetTextMeshProUGUI((int)TextMeshProUGUIs.PlayOptionText).gameObject.SetActive(true);    //�ؽ�Ʈ ��Ȱ��ȭ

            //����
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

        //FixedDeltaTime����
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }
}
