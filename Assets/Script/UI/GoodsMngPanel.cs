using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoodsMngPanel : UI_Controller
{
    [SerializeField]
    GameObject goldText;

    // ���İ�(����)
    float alpaValue;

    // �г��� �̵���ų �̵���
    float addPos = 0.1f;

    // �г��� �ʱ� ��ġ
    Vector3 initialPos;

    public string filePath;

    enum Texts
    {
        Gold
    }
    private void Start()
    {
        initialPos = transform.position;
    }
    private void Update()
    {
        if (gameObject.activeSelf == true)
            UpdatePanelPos();
    }
    protected override void BindingUI()
    {
        base.BindingUI();

        Bind<TextMeshProUGUI>(typeof(Texts));

    }

    /// <summary>
    /// ȹ���ϰų� ����� ��带 �� �� �� ȭ�鿡 UI�� ǥ��
    /// </summary>
    /// <param name="value">ȭ�鿡 ǥ���� ��差</param>
    public void ShowGold(int value, int identity)
    {        
        alpaValue = 1; // ���� ������ ���� ���İ� �ʱ�ȭ

        // text UI�� �����ͼ� �� ���� �� Ȱ��ȭ       
        TextMeshProUGUI gold = goldText.GetComponentInChildren<TextMeshProUGUI>();

        // �ڿ��� ������ų�� ���ҽ�ų�� �Ǻ�
        if (identity >= 0)
        {
            gold.text = "+" + value.ToString();
            gold.color = new Color(1, 0, 0, alpaValue);
        }
        else
        {
            gold.text = "-" + value.ToString();
            gold.color = new Color(0, 1, 0, alpaValue);
        }

            goldText.SetActive(true);

        StartCoroutine(ShowGoldCoroutine(gold, identity));
    }

    /// <summary>
    /// UI�� �ڿ������� ��Ȱ��ȭ�� ���� ���İ�(����)�� �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <param name="gold">ȭ�鿡 ǥ���� ��� UI</param>
    /// <returns></returns>
    IEnumerator ShowGoldCoroutine(TextMeshProUGUI gold, int identity)
    {
        while (true)
        {
            // ���İ��� Ư���� ���Ϸ� �������� ��Ȱ��ȭ
            if(alpaValue < 0.1f)
            {
                goldText.SetActive(false);
                SystemManager.Instance.PanelManager.DisablePanel<GoodsMngPanel>(gameObject);
                addPos = 0.1f;  // ��ġ ������Ʈ�� ���� ���� �ʱ�ȭ
                transform.position = initialPos;
                // �ڷ�ƾ ����
                StopCoroutine(ShowGoldCoroutine(gold, identity));
            }

            // ���İ��� �ٿ� ���� �����ϰ� ����
            alpaValue -= 0.02f;
            if(identity > 0)
                gold.color = new Color(0, 1, 0, alpaValue);
            else
                gold.color = new Color(1, 0, 0, alpaValue);

            yield return new WaitForSeconds(0.01f);
        }
    }

    /// <summary>
    /// UI�� �� �������� �̵��ϸ鼭 ���̵� �ƿ� �Ǵ� ȿ���� �ֱ� ���� �г� ������ ������Ʈ
    /// </summary>
    void UpdatePanelPos()
    {
        Vector3 movePos = new Vector3(transform.position.x, transform.position.y + addPos, transform.position.z);
        transform.position = Vector3.Lerp(initialPos, movePos, Time.deltaTime * 20f);
        //transform.position = new Vector3(transform.position.x, transform.position.y + addPos, transform.position.z);
        addPos += 0.5f;
    }
    
}
