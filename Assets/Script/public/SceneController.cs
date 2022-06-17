using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
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
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Single));
    }
    IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

        while (!asyncOperation.isDone)
            yield return null;

        Debug.Log("LoadSceneAsync is complete");
    }

    public void OnActiveSceneChanged(Scene scene0, Scene scene1)
    {
        Debug.Log("OnActiveSceneChanged is called! scene0 = " + scene0.name + ", scene1 = " + scene1.name);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("OnSceneLoaded is called! scene = " + scene.name + ", loadSceneMode = " + loadSceneMode.ToString());
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded is called! scene = " + scene.name);
    }
}
