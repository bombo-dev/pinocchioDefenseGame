using System.Collections;
using TMPro;
using UnityEngine;

public class KillRewardMngPanel : MonoBehaviour
{
    [SerializeField]
    GameObject RewardText;

    public string filePath;

    float alpaValue;

    float addPos = 0.1f;

    public GameObject rewardOwner = null;

    KillRewardMngPanel killRewardMngPanel;

    Coroutine runningCoroutine = null;

    private void Update()
    {
        if (gameObject.activeSelf == true)
            UpdatePanelPos();

        if (Time.timeScale == 0)
        {
            int panelNum = SystemManager.Instance.PanelManager.rewardPanels.Count;
            int i = 0;

            while (i < panelNum)
            {
                SystemManager.Instance.PanelManager.DisablePanel<KillRewardMngPanel>(SystemManager.Instance.PanelManager.rewardPanels[0]);
                i++;
            }
        }
    }
    public void ShowReward(int value)
    {
        alpaValue = 1;
        TextMeshProUGUI reward = RewardText.GetComponentInChildren<TextMeshProUGUI>();
        reward.text = "+" + value.ToString();

        //reward.color = new Color(reward.color.r, reward.color.g, reward.color.b, alpaValue);
        reward.alpha = alpaValue;

        RewardText.SetActive(true);

        runningCoroutine = StartCoroutine(ShowRwdCoroutine(value, reward));

    }

    IEnumerator ShowRwdCoroutine(int value, TextMeshProUGUI reward)
    {
        while (true)
        {
            if (alpaValue < 0.1f)
            {
                RewardText.SetActive(false);
                SystemManager.Instance.PanelManager.DisablePanel<KillRewardMngPanel>(gameObject);
                addPos = 0.1f;

                StopCoroutine(runningCoroutine);
            }
            alpaValue -= 0.03f;
            reward.alpha = alpaValue;
            yield return new WaitForSeconds(0.01f);

        }
    }

    void UpdatePanelPos()
    {
        Enemy enemy = rewardOwner.GetComponent<Enemy>();
        transform.position = Camera.main.WorldToScreenPoint(new Vector3(enemy.hpPos.transform.position.x, enemy.hpPos.transform.position.y + addPos, enemy.hpPos.transform.position.z));
        addPos += 0.1f;
    }
}
