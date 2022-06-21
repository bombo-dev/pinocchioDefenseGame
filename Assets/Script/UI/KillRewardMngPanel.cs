using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillRewardMngPanel : MonoBehaviour
{
    [SerializeField]
    GameObject RewardText;

    public string filePath;

    float alpaValue;

    float addPos = 0.1f;

    Vector3 startPos, endPos;

    public GameObject rewardOwner = null;

    private void Update()
    {
        if(gameObject.activeSelf == true)
            UpdatePanelPos();
    }
    public void ShowReward(int value)
    {
        alpaValue = 1;
        TextMeshProUGUI reward = RewardText.GetComponentInChildren<TextMeshProUGUI>();
        reward.text = "+"+ value.ToString();

        RewardText.SetActive(true);

        StartCoroutine(ShowRwdCoroutine(value));

    }

    IEnumerator ShowRwdCoroutine(int value)
    {
        while(true)
        {
            if (alpaValue < 0.1f)
            {
                RewardText.SetActive(false);
                SystemManager.Instance.PanelManager.DisablePanel<KillRewardMngPanel>(gameObject);
                addPos = 0.1f;

                StopCoroutine(ShowRwdCoroutine(value));
            }
            alpaValue -= 0.01f;

            yield return new WaitForSeconds(0.01f);

        }
    }

    void UpdatePanelPos()
    {
        Enemy enemy = rewardOwner.GetComponent<Enemy>();
        //startPos = Camera.main.WorldToScreenPoint(enemy.hitPos.transform.position);
        //endPos = Camera.main.WorldToScreenPoint(new Vector3(enemy.hitPos.transform.position.x, enemy.hitPos.transform.position.y + addPos, enemy.hitPos.transform.position.z));

        //transform.position = Vector3.Lerp(startPos, endPos, Time.deltaTime * 5f);
        transform.position = Camera.main.WorldToScreenPoint(new Vector3(enemy.hpPos.transform.position.x, enemy.hpPos.transform.position.y+addPos, enemy.hpPos.transform.position.z));
        addPos += 0.1f;
    }
}
