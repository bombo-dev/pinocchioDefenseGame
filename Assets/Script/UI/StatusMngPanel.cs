using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StatusMngPanel : UI_Controller
{   

    public int enemyHPBarIndex;  // Ȱ��ȭ�� ���ʹ� �г��� �迭 �ε���

    public int turretHPBarIndex; // Ȱ��ȭ�� �ͷ� �г��� �迭 �ε���

    public string filePath;

    public Vector3 panelPos;

    public GameObject hpBarOwner;

    [SerializeField]
    GameObject[] Debuffs;

    [SerializeField]
    Image Fill;

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
    
    public void SetHPBarColor()
    {
        //Fill.color = Color.blue; //�Ķ�
        //Fill.color = new Color(255 / 255f, 121 / 255f, 0 / 255f); //��Ȳ
        Fill.color = Color.green;
    }

    public void SetHPBar(float currentHP, float maxHP)
    {
        if (currentHP <= 0)
            return;

        if (currentHP > maxHP)
            currentHP = maxHP;

        currentHP /= maxHP;

        GetSlider((int)Sliders.HPBar).value = currentHP;

    }


    public void SetDebuff(int debuffIdx, Dictionary<Actor.debuff, Debuff> debuffs , float time)
    {
        if (debuffIdx <= 0)
            return;

        Debug.Log("debuffIdx="+debuffIdx);

        GameObject go = Debuffs[debuffIdx-1];
        go.SetActive(true);
        
        TextMeshProUGUI debuffText = go.GetComponentInChildren<TextMeshProUGUI>();

        int stack = debuffs[(Actor.debuff)debuffIdx].stack;

        if (stack >= 2)
            debuffText.text = "X"+ stack.ToString();
      
    }

    public void RemoveDebuff(int debuffIndex, Dictionary<Actor.debuff, Debuff> debuffs)
    {
        GameObject go = Debuffs[debuffIndex-1];
        go.SetActive(false);
    }

    public void StatusReset()
    {
        // HPbar ���� Reset
        GetSlider((int)Sliders.HPBar).value = 1.0f/1.0f;
        Fill.color = Color.red;

        //����� ���� Reset
        for(int i=0; i<Debuffs.Length; i++)
        {
            Debuffs[i].SetActive(false);
            Debuffs[i].GetComponentInChildren<TextMeshProUGUI>().text = null;
        }

    }
}
