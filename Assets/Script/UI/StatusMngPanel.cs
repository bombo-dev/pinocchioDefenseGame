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

    public GameObject hpBarOwner;   // hpBar�� �����ִ� ����

    public Sprite greenBar; 

    [SerializeField]
    GameObject[] Debuffs;

    [SerializeField]
    Image Fill;

    float debuffFlowTime = 0.0f;

    float debuffLeftTime;

    public int randPos;    // �������� ������ y�� ��

    public float hp;

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
        Fill.sprite = greenBar;
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

        hp = currentHP;
    }

    /// <summary>
    /// ����� ����
    /// </summary>
    /// <param name="debuffIdx"></param>
    /// <param name="debuffs"></param>
    /// <param name="time"></param>
    public void SetDebuff(int debuffIdx, Dictionary<Actor.debuff, Debuff> debuffs , float time)
    {
        // ����ó��
        if (debuffIdx <= 0 || hp <= 0)
            return;

        if (gameObject.activeSelf == false)
            return;

        // �ش� �ε����� ����� ��������
        GameObject go = Debuffs[debuffIdx - 1];

        if (!go)
            return;

        // ����� �ؽ�Ʈ ��������
        TextMeshProUGUI debuffText = go.GetComponentInChildren<TextMeshProUGUI>();

        // ���� ���� �޾ƿ���
        int stack = debuffs[(Actor.debuff)debuffIdx].stack;        

        // ����� ������ ������Ʈ ��������
        Transform GoTransform = go.transform.GetChild(0).transform.GetChild(0);
        Image ImgFillAmount = GoTransform.GetComponent<Image>();

        // ������� ����ð��� ī��Ʈ �� ���� �ʱ�ȭ
        debuffFlowTime = 0.0f;

        // ����� ��ø��
        if (stack >= 2)
        {
            // ������ ����Ǵ� �ڷ�ƾ ����
            StopCoroutine(DebuffCoroutine(ImgFillAmount, time, debuffIdx));

            // ��ø ������ ȭ�鿡 ǥ��
            debuffText.text = "X" + stack.ToString();
        }
        else
        {
            // ����� ��ø �ؽ�Ʈ�� �������� ǥ��
            debuffText.text = " ";
        }

        // ����� UI Ȱ��ȭ
        go.SetActive(true);

        // �ڷ�ƾ ����
        StartCoroutine(DebuffCoroutine(ImgFillAmount, time, debuffIdx));
    }
    
    /// <summary>
    /// ����� UI ��� ���� �ڷ�ƾ
    /// </summary>
    /// <param name="image">����� �������� ǥ���� �̹���</param>
    /// <param name="time">����� ���ӽð�</param>
    /// <param name="debuffIdx">����� �ε���</param>
    /// <returns></returns>
    IEnumerator DebuffCoroutine(Image image, float time, int debuffIdx)
    {
        while (true)
        {
            // ���ӽð��� �ٵ��� �� or �г��� ��Ȱ��ȭ ������ ��
            if (debuffFlowTime >= time || gameObject.activeSelf == false)
            {
                // �ڷ�ƾ ����
                StopCoroutine(DebuffCoroutine(image, time, debuffIdx));               

                // ����� �г� ��Ȱ��ȭ
                RemoveDebuff(image, time, debuffIdx);

            }
            Debug.Log("time=" + time);

            // ����� ����ð� ī��Ʈ
            debuffFlowTime += Time.deltaTime;

            // ����� ����ð��� ������ UI�� ǥ��
            image.fillAmount = (debuffFlowTime/time);

            yield return new WaitForSeconds(Time.deltaTime);
                 
        }
    }
    
    public void RemoveDebuff(Image image, float time, int debuffIndex)
    {
        
        GameObject go = Debuffs[debuffIndex-1];
        go.SetActive(false);
        debuffLeftTime = 0.0f;
        
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
