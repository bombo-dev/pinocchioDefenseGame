using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatusMngPanel : UI_Controller
{
    const int MAXDEBUFF = 4;    // �ִ� ����� ��    

    public int enemyHPBarIndex;  // Ȱ��ȭ�� ���ʹ� �г��� �迭 �ε���

    public int turretHPBarIndex; // Ȱ��ȭ�� �ͷ� �г��� �迭 �ε���

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
