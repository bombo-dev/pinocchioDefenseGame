using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [Header("Nest")]
    //현재 선택한 둥지 오브젝트
    public GameObject currenstSelectNest;
    [SerializeField]
    List<Renderer> rendererList;

    //선택한 둥지 강조 오브젝트
    [SerializeField]
    GameObject nestPointer;

    // Start is called before the first frame update
    void Start()
    {
        if(nestPointer.activeSelf)
            nestPointer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                TouchObject();
        }
        else 
            TouchObject();
    }

    /// <summary>
    /// 터치 혹은 마우스 클릭을 이용해 오브젝트 선택 : 김현진
    /// </summary>
    void TouchObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("터치");

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit);

                if (hit.collider != null)
                {
                    //터렛을 소환하거나 터렛 정보를 확인할 둥지 오브젝트 선택
                    SelectNest(hit.transform.gameObject);

                    //사거리 표시
                    if (currenstSelectNest)
                        ShowRange();
                }

            }
        }

        /*
        if (Input.GetMouseButtonUp(0))
        {
            //예외처리
            if (!currenstSelectNest)
                return;

            SystemManager.Instance.ShaderController.ChangeOutLineOption(rendererList, 0);
            rendererList.Clear();

            //오브젝트 선택 해제
            currenstSelectNest = null;

            Debug.Log("터치업");
        }*/
    }

    /// <summary>
    /// 터렛을 소환할 둥지를 선택한다 : 김현진
    /// </summary>
    ///<param name="hitObject">선택할 게임 오브젝트</param>
    void SelectNest(GameObject hitGo)
    {
        //이전에 선택한 오브젝트 효과초기화
        if (currenstSelectNest != null)
            OffHightlightObject(currenstSelectNest);

        //오브젝트 선택
        if(hitGo != null)
            currenstSelectNest = hitGo;

        Nest nest = currenstSelectNest.GetComponent<Nest>();

        if (nest != null)
        {
            if(SystemManager.Instance.PanelManager.turretInfoPanel)
                SystemManager.Instance.PanelManager.turretInfoPanel.GetComponent<UI_TurretInfoPanel>().Reset();
        }

        //둥지 포인터 활성화
        if (!nestPointer.activeSelf)
        { 
            nestPointer.SetActive(true);
        }
        nestPointer.transform.position = currenstSelectNest.transform.position;

        //선택한 오브젝트 하이라이트 효과
        OnHighlightObject(currenstSelectNest);
    }


    /// <summary>
    /// 게임 오브젝트의 쉐이더 외곽선을 통해 하이라이트 효과를 켜는 함수 : 김현진
    /// </summary>
    /// <param name="go">하이라이트 효과를 표현할 게임 오브젝트</param>
    void OnHighlightObject(GameObject go)
    {
        rendererList.Clear();

        Renderer renderer = go.GetComponent<Renderer>();

        if (renderer != null)
        {
            rendererList.Add(go.GetComponent<Renderer>());
            SystemManager.Instance.ShaderController.ChangeOutLineOption(rendererList, 2);
        }
    }

    /// <summary>
    /// 게임 오브젝트의 쉐이더 외곽선을 통해 하이라이트 효과를 끄는 함수 : 김현진
    /// </summary>
    /// <param name="go">하이라이트 효과를 표현할 게임 오브젝트</param>
    void OffHightlightObject(GameObject go)
    {
        rendererList.Clear();

        Renderer renderer = go.GetComponent<Renderer>();

        if (renderer != null)
        {
            rendererList.Add(go.GetComponent<Renderer>());
            SystemManager.Instance.ShaderController.ChangeOutLineOption(rendererList, 0);
        }
       
    }

    //사거리 보여주기
    public void ShowRange()
    {
        //예외처리
        if (!SystemManager.Instance.RangeManager.rangeParents)
        {
            SystemManager.Instance.RangeManager.DisableRange(0);
            return;
        }

        Nest nest = currenstSelectNest.GetComponent<Nest>();

        //예외처리
        if (!nest || !nest.turret)
        {
            SystemManager.Instance.RangeManager.DisableRange(0);
            return;
        }

        Turret turret = nest.turret.GetComponent<Turret>();

        //예외처리
        if (!turret)
        {
            SystemManager.Instance.RangeManager.DisableRange(0);
            return;
        }

        //사거리 표시
        SystemManager.Instance.RangeManager.EnableRange(0, turret.currentRange, nest.transform.position);
    }
}
