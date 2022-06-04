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

    enum Texts
    {
        Damage
    }

    protected override void BindingUI()
    {
        Bind<Text>(typeof(Texts));
    }

    public void ShowDamage(int power)
    {
        
        float alpaValue = 1;  // 투명도 조절을 위한 알파값      
        TextMeshProUGUI damage = DamageText.GetComponentInChildren<TextMeshProUGUI>();
        damage.text = power.ToString();        
        DamageText.SetActive(true);

        StartCoroutine(ShowDmgCoroutine(damage, alpaValue));

    }
    IEnumerator ShowDmgCoroutine(TextMeshProUGUI damage, float alpaValue)
    {
        // 알파값(투명도) 조절
        while (alpaValue >= 0.0f)
        {
            if (alpaValue < 0.1)
            {
                DamageText.SetActive(false);
                SystemManager.Instance.PanelManager.DisablePanel<DamageMngPanel>(this.gameObject);
            }
            alpaValue -= 0.01f;            
            damage.color = new Color(1, 0, 0, alpaValue);
            yield return new WaitForSeconds(0.01f);
            //damage.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z));
        }
                
        //yield return new WaitForSeconds(delayTime);
        

    }

}
