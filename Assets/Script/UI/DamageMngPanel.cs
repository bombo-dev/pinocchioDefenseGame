using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageMngPanel : UI_Controller
{
    [SerializeField]
    GameObject DamageText;

    public string filePath;

    public float alpaValue;

    public GameObject damageOwner = null;

    float addPos = 0.1f;

    Vector3 screenPos; // 패널 위치 업데이트를 위한 포스값

    DamageMngPanel damageMngPanel;

    public int randPosX; // 랜덤으로 더해질 x축 값
    public int randPosY; // 랜덤으로 더해질 y축 값

    enum Texts
    {
        Damage
    }

    private void Update()
    {
        if(gameObject.activeSelf == true)
            UpdatePanelPos();    
            
        if(Time.timeScale == 0)
        {
            int panelNum = SystemManager.Instance.PanelManager.damagePanels.Count;
            Debug.Log("panelNum=" + panelNum);
            int i = 0;

            while (i < panelNum)
            {
                damageMngPanel = SystemManager.Instance.PanelManager.damagePanels[0].GetComponent<DamageMngPanel>();
                //damageMngPanel.gameObject.SetActive(false);
                damageMngPanel.DisableDmgPanel(null, 0);
                i++;
            }
        }
    }
    protected override void BindingUI()
    {
        Bind<Text>(typeof(Texts));
    }

    public void SetTextColor(TextMeshProUGUI damage, int identity)
    {
        if (identity == 1)
            damage.color = new Color(0, 1, 0, alpaValue);
        else if (identity == 0)
            damage.color = new Color(1, 0, 0, alpaValue);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="power">데미지량/회복량</param>
    /// <param name="identity"> 식별 변수. 0: 데미지, 1: 회복</param>
    public void ShowDamage(int power, int identity)
    {
        
        alpaValue = 1;  // 투명도 조절을 위한 알파값 초기화 
        TextMeshProUGUI damage = DamageText.GetComponentInChildren<TextMeshProUGUI>();        
        damage.text = power.ToString();

        // 텍스트 색상 설정
        SetTextColor(damage, identity);

        DamageText.SetActive(true);

        StartCoroutine(ShowDmgCoroutine(damage, identity));

    }
    IEnumerator ShowDmgCoroutine(TextMeshProUGUI damage, int identity)
    {
        // 알파값(투명도) 조절
        //while (alpaValue >= 0.0f)
        while(true)
        {
            // 알파값이 특정값 이하로 내려가면 비활성화
            if (alpaValue < 0.1)
            {
                DisableDmgPanel(damage, identity);

            }
            alpaValue -= 0.03f;

            SetTextColor(damage, identity);            
            
            yield return new WaitForSeconds(0.01f);            
        }
    }

    public void DisableDmgPanel(TextMeshProUGUI damage, int identity)
    {
        DamageText.SetActive(false);
        SystemManager.Instance.PanelManager.DisablePanel<DamageMngPanel>(gameObject);

        // 활성화된 데미지 패널을 저장한 리스트에서 해당 패널 제거
        //SystemManager.Instance.PanelManager.damagePanels.Remove(gameObject);

        addPos = 0.1f;  // 위치 업데이트를 위한 변수 초기화

        // 코루틴 종료
        StopCoroutine(ShowDmgCoroutine(damage, identity));
    }

    void UpdatePanelPos()
    {
        //Debug.Log("RandPosX=" + randPosX);
        //Debug.Log("RandPosY=" + randPosY);

        if (damageOwner.tag == "Turret")
        {
            Turret turret = damageOwner.GetComponent<Turret>();
            screenPos = Camera.main.WorldToScreenPoint(new Vector3(turret.hitPos.transform.position.x + randPosX, 
                                                                                                          turret.hitPos.transform.position.y+ randPosY + addPos, 
                                                                                                          turret.hitPos.transform.position.z));            
            //screenPos = turret.hitPos.transform.position;
        }
        else if (damageOwner.tag == "Enemy")
        {
            Enemy enemy = damageOwner.GetComponent<Enemy>();
            screenPos = Camera.main.WorldToScreenPoint(new Vector3(enemy.hitPos.transform.position.x + randPosX, 
                                                                                                          enemy.hitPos.transform.position.y + randPosY + addPos, 
                                                                                                          enemy.hitPos.transform.position.z));            
            //screenPos = enemy.hitPos.transform.position;
        }
        else
            return;

        transform.position = screenPos;
        //transform.Translate(Vector3.up * 10f * Time.deltaTime);        
        addPos += 0.1f;
    }

}
