using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageMngPanel : UI_Controller
{
    [SerializeField]
    GameObject DamageText;

    float delayTime = 1.3f;

    public string filePath;

    float alpaValue;

    public GameObject damageOwner;

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

    public void ShowDamage(int power)
    {
        
        alpaValue = 1;  // 투명도 조절을 위한 알파값      
        TextMeshProUGUI damage = DamageText.GetComponentInChildren<TextMeshProUGUI>();
        damage.text = power.ToString();        
        DamageText.SetActive(true);

        StartCoroutine(ShowDmgCoroutine(damage));

    }
    IEnumerator ShowDmgCoroutine(TextMeshProUGUI damage)
    {
        // 알파값(투명도) 조절
        //while (alpaValue >= 0.0f)
        while(true)
        {
            if (alpaValue < 0.1)
            {
                DamageText.SetActive(false);
                SystemManager.Instance.PanelManager.DisablePanel<DamageMngPanel>(gameObject);
                addPos = 0.1f;
                StopCoroutine(ShowDmgCoroutine(damage));
            }
            alpaValue -= 0.01f;            
            damage.color = new Color(1, 0, 0, alpaValue);

            
            yield return new WaitForSeconds(0.01f);
            //damage.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z));
        }
                
        //yield return new WaitForSeconds(delayTime);
        

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
