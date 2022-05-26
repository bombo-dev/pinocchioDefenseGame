using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatusMngPanel : UI_Controller
{
    const int MAXDEBUFF = 4;    // 최대 디버프 수    

    public int enemyHPBarIndex;  // 활성화된 에너미 패널의 배열 인덱스

    public int turretHPBarIndex; // 활성화된 터렛 패널의 배열 인덱스

    public string filePath;
    
    enum Images
    {
        Debuff_1_img,
        Debuff_2_img,
        Debuff_3_img,
        Debuff_4_img,
    }

    public enum Sliders
    {
        HPBar
    }

    protected override void BindingUI()
    {
        Bind<Slider>(typeof(Sliders));

        base.BindingUI();

    }

    public void SetHPBar(float currentHP, float maxHP)
    {
        if (currentHP > maxHP)
            currentHP = maxHP;

        currentHP /= maxHP;
        //Debug.Log("currentHP= " + currentHP);
        GetSlider((int)Sliders.HPBar).value = currentHP;
        //Debug.Log("slider.value= "+ GetSlider((int)Sliders.HPBar).value);
    }

    
}
