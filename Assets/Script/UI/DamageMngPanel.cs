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

    float alpaValue;

    public GameObject damageOwner = null;

    float addPos = 0.1f;

    enum Texts
    {
        Damage
    }

    private void Update()
    {
        if(gameObject.activeSelf == true)
            UpdatePanelPos();
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
                DamageText.SetActive(false);
                SystemManager.Instance.PanelManager.DisablePanel<DamageMngPanel>(gameObject);
                addPos = 0.1f;  // 위치 업데이트를 위한 변수 초기화

                // 코루틴 종료
                StopCoroutine(ShowDmgCoroutine(damage, identity));
            }
            alpaValue -= 0.01f;

            SetTextColor(damage, identity);            
            
            yield return new WaitForSeconds(0.01f);            
        }
                

        

    }

    void UpdatePanelPos()
    {
        if (damageOwner.tag == "Turret")
        {
            Turret turret = damageOwner.GetComponent<Turret>();
            Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(turret.hitPos.transform.position.x, turret.hitPos.transform.position.y+addPos, turret.hitPos.transform.position.z));
            transform.position = screenPos;

        }
        else if(damageOwner.tag == "Enemy")
        { 
            Enemy enemy = damageOwner.GetComponent<Enemy>();
            Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(enemy.hitPos.transform.position.x, enemy.hitPos.transform.position.y + addPos, enemy.hitPos.transform.position.z));
            transform.position = screenPos;
        }
        addPos += 0.1f;
    }

}
