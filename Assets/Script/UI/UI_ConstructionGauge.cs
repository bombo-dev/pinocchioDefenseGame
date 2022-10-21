using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ConstructionGauge : UI_Controller
{
    public string filePath;

    //����� �ͷ� ����
    public ConstructionTurret constructionTurret;

    public enum Sliders
    {
        ConstructionGauge
    }

    Slider constructionSlide;

    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Slider>(typeof(Sliders));

        constructionSlide = GetSlider((int)Sliders.ConstructionGauge);//���� ������ ���� �޾ƿ���
    }

    private void Update()
    {
        UpdateSlideBar();
    }

    /// <summary>
    /// �����̵���� ���� ��ġ�� �ǽð����� ���� ���ش� : ������
    /// </summary>
    void UpdateSlideBar()
    {
        //��ġ����
        transform.position = Camera.main.WorldToScreenPoint(constructionTurret.gauegePos.transform.position);

        //������
        constructionSlide.value = constructionTurret.constructionValue;
    }

}
