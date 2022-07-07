using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;

    [SerializeField]
    Image progressBar;

    [SerializeField]
    TextMeshProUGUI tipText;

    [SerializeField]
    string[] tip;

    [SerializeField]
    Sprite[] loadingImageArr;

    [SerializeField]
    Image loadingImage;

    private void Start()
    {
        ChangeTipText();

        ChangeLoadingImage();
    }

    /// <summary>
    /// 팁 텍스트 랜덤으로 교체 : 김현진
    /// </summary>
    void ChangeTipText()
    {
        tipText.text = tip[Random.Range(0, tip.Length)];
    }

    /// <summary>
    /// 로딩 이미지 랜덤으로 교체 : 김현진
    /// </summary>
    void ChangeLoadingImage()
    {
        loadingImage.sprite = loadingImageArr[Random.Range(0, loadingImageArr.Length)];
    }


    /// <summary>
    /// 로딩바 업데이트 : 김현진
    /// </summary>
    /// <param name="progress">로딩 진행 상태</param>
    /// <param name="timer">로딩바 업데이트 시간</param>
    public void UpdateProgressBar(float progress, float timer)
    {
        progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progress, timer);
        if (progressBar.fillAmount >= progress)
        { 
            timer = 0f; 
        }
    }

    /// <summary>
    /// 로딩바 업데이트 완료 : 김현진
    /// </summary>
    /// <param name="timer">로딩바 완료 시간</param>
    /// <returns>로딩완료 상태</returns>
    public bool FinProgressBar(float timer)
    {
        progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
        if (progressBar.fillAmount == 1.0f)
        {
            return true;
        }
        return false;
    }

    /*public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            if (op.progress < 0.9f) 
            { 
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress) 
                { timer = 0f; }
            } 
            else { 
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true; yield break;
                }
            }
        }
    }*/
}
