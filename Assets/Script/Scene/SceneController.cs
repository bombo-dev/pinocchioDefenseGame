using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //씬이름
    public string gameSceneName = "GameScene";  //게임 씬
    public string lobbySceneName = "LobbyScene";    //로비 씬
    public string storySceneName = "StoryScene";    //스토리 씬

    private static SceneController instance = null;

    public static SceneController Instance
    {
        get//씬에 배치되있지 않아도 쓸수있는 싱글턴
        {
            if (instance == null)
            {
                //최초 사용시 클래스명과 같은 이름의 게임오브젝트를 만들어서 클래스 추가
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
        //Scene변화에 따른 이벤트 메소드를 매핑
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    /// <summary>
    /// 이전 Scene을 Unload하고 로딩 : 김현진
    /// </summary>
    /// <param name="sceneName"> 로딩할 scene 이름 </param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName)); 
        //로딩씬 호출
        SceneManager.LoadScene("LoadingScene");

    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        //로딩 완료시 자동로드 X
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
    /// 씬이 교체되었을때 : 김현진
    /// </summary>
    /// <param name="scene0"></param>
    /// <param name="scene1">교체된 씬</param>
    public void OnActiveSceneChanged(Scene scene0, Scene scene1)
    {
        Debug.Log("OnActiveSceneChanged is called! scene0 = " + scene0.name + ", scene1 = " + scene1.name);
    }

    /// <summary>
    /// 로드된 씬: 김현진
    /// </summary>
    /// <param name="scene">교체된 씬</param>
    /// <param name="loadSceneMode">씬로드 방식</param>
    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("OnSceneLoaded is called! scene = " + scene.name + ", loadSceneMode = " + loadSceneMode.ToString());
        SystemManager.Instance.Initialize();
    }

    /// <summary>
    /// 씬이 종료되었을때 : 김현진
    /// </summary>
    /// <param name="scene">종료된 씬</param>
    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded is called! scene = " + scene.name);
    }
}
