using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //���̸�
    public string gameSceneName = "GameScene";  //���� ��
    public string lobbySceneName = "LobbyScene";    //�κ� ��
    public string storySceneName = "StoryScene";    //���丮 ��

    private static SceneController instance = null;

    public static SceneController Instance
    {
        get//���� ��ġ������ �ʾƵ� �����ִ� �̱���
        {
            if (instance == null)
            {
                //���� ���� Ŭ������� ���� �̸��� ���ӿ�����Ʈ�� ���� Ŭ���� �߰�
                GameObject go = GameObject.Find("SceneController");
                if (go == null)
                {
                    go = new GameObject("SceneController");

                    SceneController controller = go.AddComponent<SceneController>();
                    return controller;
                }
                else
                {
                    instance = go.GetComponent<SceneController>();
                }

            }
            return instance;
        }
    }

    void Awake()
    {
        //Don't Destroy
        if (instance != null)
        {
            Debug.LogWarning("Can't have two instance of singletone.");
            DestroyImmediate(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Event
        //Scene��ȭ�� ���� �̺�Ʈ �޼ҵ带 ����
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    /// <summary>
    /// ���� Scene�� Unload�ϰ� �ε� : ������
    /// </summary>
    /// <param name="sceneName"> �ε��� scene �̸� </param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName)); 
        //�ε��� ȣ��
        SceneManager.LoadScene("LoadingScene");

    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        //�ε� �Ϸ�� �ڵ��ε� X
        op.allowSceneActivation = false;
        float timer = 0.0f;

        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                Debug.Log(op.progress);
                SystemManager.Instance.LoadingSceneManager.UpdateProgressBar(op.progress, timer);
            }
            else
            {
                if (SystemManager.Instance.LoadingSceneManager.FinProgressBar(timer))
                {
                    op.allowSceneActivation = true; 
                    yield break;
                }
            }
        }

        Debug.Log("LoadSceneAsync is complete");
    }

    /// <summary>
    /// ���� ��ü�Ǿ����� : ������
    /// </summary>
    /// <param name="scene0"></param>
    /// <param name="scene1">��ü�� ��</param>
    public void OnActiveSceneChanged(Scene scene0, Scene scene1)
    {
        Debug.Log("OnActiveSceneChanged is called! scene0 = " + scene0.name + ", scene1 = " + scene1.name);
    }

    /// <summary>
    /// �ε�� ��: ������
    /// </summary>
    /// <param name="scene">��ü�� ��</param>
    /// <param name="loadSceneMode">���ε� ���</param>
    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("OnSceneLoaded is called! scene = " + scene.name + ", loadSceneMode = " + loadSceneMode.ToString());
        SystemManager.Instance.Initialize();
    }

    /// <summary>
    /// ���� ����Ǿ����� : ������
    /// </summary>
    /// <param name="scene">����� ��</param>
    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded is called! scene = " + scene.name);
    }
}
