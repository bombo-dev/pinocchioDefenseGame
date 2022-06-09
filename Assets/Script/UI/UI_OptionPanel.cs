using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_OptionPanel : UI_Controller
{
    [SerializeField]
    Sprite timeScaleImage_1;
    [SerializeField]
    Sprite timeScaleImage_1_2;
    [SerializeField]
    Sprite timeScaleImage_1_5;
    [SerializeField]
    Sprite timeScaleImage_2;
    [SerializeField]
    Sprite stopImage;
    [SerializeField]
    Sprite playImage;

    float currentTimeScale;

    enum Buttons
    {
        DoubleSpeedOptionButton, //��� �ɼ�,
        PlayOptionButton    //��� �ɼ�
    }

    /// <summary>
    /// enum�� ���ŵ� �̸����� UI������ ���ε� : ������
    /// </summary>
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Button>(typeof(Buttons));

        //Ÿ�ӽ����� �ʱ�ȭ
        GetButton((int)Buttons.DoubleSpeedOptionButton).image.sprite = timeScaleImage_1;    //�̹��� ��ü
        GetButton((int)Buttons.PlayOptionButton).image.sprite = stopImage;    //�̹��� ��ü
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
            //�̹��� ��ü
            GetButton((int)Buttons.DoubleSpeedOptionButton).image.sprite = timeScaleImage_1_2;

            //1.2������� ����
            Time.timeScale = 1.2f;
            currentTimeScale = 1.2f;
        }
        else if (Time.timeScale == 1.2f)
        {
            //�̹��� ��ü
            GetButton((int)Buttons.DoubleSpeedOptionButton).image.sprite = timeScaleImage_1_5;

            //1.5������� ����
            Time.timeScale = 1.5f;
            currentTimeScale = 1.5f;
        }
        else if (Time.timeScale == 1.5f)
        {
            //�̹��� ��ü
            GetButton((int)Buttons.DoubleSpeedOptionButton).image.sprite = timeScaleImage_2;

            //1.5������� ����
            Time.timeScale = 2.0f;
            currentTimeScale = 2.0f;
        }
        else
        {
            //�̹��� ��ü
            GetButton((int)Buttons.DoubleSpeedOptionButton).image.sprite = timeScaleImage_1;

            //1.5������� ����
            Time.timeScale = 1.0f;
            currentTimeScale = 1.0f;
        }

        //FixedDeltaTime����
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    /// <summary>
    /// ���� �����϶� �ٽ� ����ϰ�, ��� �����϶� �ٽ� �����Ѵ�
    /// </summary>
    /// <param name="data"></param>
    public void OnClickPlayOptionButton(PointerEventData data)
    {
        //���� ������ ���
        if (Time.timeScale == 0f)
        {
            //�̹��� ��ü
            GetButton((int)Buttons.PlayOptionButton).image.sprite = stopImage;

            //���
            Time.timeScale = currentTimeScale;
        }
        //���� ������ ���
        else
        {
            //�̹��� ��ü
            GetButton((int)Buttons.PlayOptionButton).image.sprite = playImage;

            //����
            Time.timeScale = 0;
        }

        //FixedDeltaTime����
        Time.fixedDeltaTime = 0.02F * Time.timeScale;

    }
}
