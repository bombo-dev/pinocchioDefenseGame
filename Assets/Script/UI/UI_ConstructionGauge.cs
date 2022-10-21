using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ConstructionGauge : UI_Controller
{
    public string filePath;

    //공사용 터렛 정보
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

        constructionSlide = GetSlider((int)Sliders.ConstructionGauge);//공사 게이지 정보 받아오기
    }

    private void Update()
    {
        UpdateSlideBar();
    }

    /// <summary>
    /// 슬라이드바의 값과 위치를 실시간으로 갱신 해준다 : 김현진
    /// </summary>
    void UpdateSlideBar()
    {
        //위치갱신
        transform.position = Camera.main.WorldToScreenPoint(constructionTurret.gauegePos.transform.position);

        //값갱신
        constructionSlide.value = constructionTurret.constructionValue;
    }

}
