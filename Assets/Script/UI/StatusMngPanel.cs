using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatusMngPanel : UI_Controller
{   

    public int enemyHPBarIndex;  // Ȱ��ȭ�� ���ʹ� �г��� �迭 �ε���

    public int turretHPBarIndex; // Ȱ��ȭ�� �ͷ� �г��� �迭 �ε���

    public string filePath;


    enum Images
    {
        Debuff_1_img,
        Debuff_2_img,
        Debuff_3_img,
        Debuff_4_img,
        Debuff_5_img,
        Debuff_6_img
    }

    enum Texts
    {
        Text_1,
        Text_2,
        Text_3,
        Text_4,
        Text_5,
        Text_6
    }
    public enum Sliders
    {
        HPBar
    }

    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<Image>(typeof(Images));
        Bind<TextMesh>(typeof(Texts));
        Bind<Slider>(typeof(Sliders));


    }

    public void SetHPBar(float currentHP, float maxHP)
    {
        if (currentHP > maxHP)
            currentHP = maxHP;

        currentHP /= maxHP;

        GetSlider((int)Sliders.HPBar).value = currentHP;
    }


    public void SetDebuff(int debuffIdx, Dictionary<Actor.debuff, Debuff> debuffs , float time)
    {
        
        //Debug.Log("go=" + GetImage(debuffIdx).gameObject);


        /*
        while (time > 0)
        {
            GetTextMeshProUGUI(debuffIdx).text = "X"+ debuffs[(Actor.debuff)debuffIdx].stack.ToString();
            go.SetActive(true);
            time--;
        }

        if (time <= 0)
            go.SetActive(false);
        */
    }
    

}
