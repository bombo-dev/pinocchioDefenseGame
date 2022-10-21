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
    /// �� �ؽ�Ʈ �������� ��ü : ������
    /// </summary>
    void ChangeTipText()
    {
        tipText.text = tip[Random.Range(0, tip.Length)];
    }

    /// <summary>
    /// �ε� �̹��� �������� ��ü : ������
    /// </summary>
    void ChangeLoadingImage()
    {
        loadingImage.sprite = loadingImageArr[Random.Range(0, loadingImageArr.Length)];
    }


    /// <summary>
    /// �ε��� ������Ʈ : ������
    /// </summary>
    /// <param name="progress">�ε� ���� ����</param>
    /// <param name="timer">�ε��� ������Ʈ �ð�</param>
    public void UpdateProgressBar(float progress, float timer)
    {
        progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progress, timer);
        if (progressBar.fillAmount >= progress)
        { 
            timer = 0f; 
        }
    }

    /// <summary>
    /// �ε��� ������Ʈ �Ϸ� : ������
    /// </summary>
    /// <param name="timer">�ε��� �Ϸ� �ð�</param>
    /// <returns>�ε��Ϸ� ����</returns>
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
