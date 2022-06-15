using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoodsMngPanel : UI_Controller
{
    [SerializeField]
    GameObject goldText;

    // 알파값(투명도)
    float alpaValue;

    // 패널을 이동시킬 이동량
    float addPos = 0.1f;

    // 패널의 초기 위치
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
    /// 획득하거나 사용한 골드를 몇 초 간 화면에 UI로 표시
    /// </summary>
    /// <param name="value">화면에 표시할 골드량</param>
    public void ShowGold(int value, int identity)
    {        
        alpaValue = 1; // 투명도 조절을 위한 알파값 초기화

        // text UI를 가져와서 값 저장 후 활성화       
        TextMeshProUGUI gold = goldText.GetComponentInChildren<TextMeshProUGUI>();

        // 자원을 증가시킬지 감소시킬지 판별
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
    /// UI의 자연스러운 비활성화를 위해 알파값(투명도)을 조절하는 코루틴
    /// </summary>
    /// <param name="gold">화면에 표시할 골드 UI</param>
    /// <returns></returns>
    IEnumerator ShowGoldCoroutine(TextMeshProUGUI gold, int identity)
    {
        while (true)
        {
            // 알파값이 특정값 이하로 내려가면 비활성화
            if(alpaValue < 0.1f)
            {
                goldText.SetActive(false);
                SystemManager.Instance.PanelManager.DisablePanel<GoodsMngPanel>(gameObject);
                addPos = 0.1f;  // 위치 업데이트를 위한 변수 초기화
                transform.position = initialPos;
                // 코루틴 종료
                StopCoroutine(ShowGoldCoroutine(gold, identity));
            }

            // 알파값을 줄여 점점 투명하게 설정
            alpaValue -= 0.02f;
            if(identity > 0)
                gold.color = new Color(0, 1, 0, alpaValue);
            else
                gold.color = new Color(1, 0, 0, alpaValue);

            yield return new WaitForSeconds(0.01f);
        }
    }

    /// <summary>
    /// UI가 윗 방향으로 이동하면서 페이드 아웃 되는 효과를 주기 위한 패널 포지션 업데이트
    /// </summary>
    void UpdatePanelPos()
    {
        Vector3 movePos = new Vector3(transform.position.x, transform.position.y + addPos, transform.position.z);
        transform.position = Vector3.Lerp(initialPos, movePos, Time.deltaTime * 20f);
        //transform.position = new Vector3(transform.position.x, transform.position.y + addPos, transform.position.z);
        addPos += 0.5f;
    }
    
}
