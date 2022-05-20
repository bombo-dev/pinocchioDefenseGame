using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Nest")]
    //현재 선택한 둥지 오브젝트
    public GameObject currenstSelectNest;
    [SerializeField]
    List<Renderer> rendererList;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        TouchObject();
    }

    /// <summary>
    /// 터치 혹은 마우스 클릭을 이용해 오브젝트 선택 : 김현진
    /// </summary>
    void TouchObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("터치");

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.collider != null)
            {
                //터렛을 소환하거나 터렛 정보를 확인할 둥지 오브젝트 선택
                SelectNest(hit.transform.gameObject);
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
        if(currenstSelectNest != null)
            OffHightlightObject(currenstSelectNest);

        //오브젝트 선택
        if(hitGo != null)
            currenstSelectNest = hitGo;

        Nest nest = currenstSelectNest.GetComponent<Nest>();

        if (nest != null)
        {
            //터렛이 설치된 Nest의경우
            if (nest.haveTurret)
            {
                //UI_TurretMngPanel 패널이 존재할 경우
                if (SystemManager.Instance.PanelManager.turretMngPanel)
                {
                    //패널 비활성화
                    SystemManager.Instance.PanelManager.DisablePanel<UI_TurretMngPanel>(SystemManager.Instance.PanelManager.turretMngPanel.gameObject);
                }

                if (!SystemManager.Instance.PanelManager.turretMngPanel)
                    SystemManager.Instance.PanelManager.EnablePanel<UI_TurretInfoPanel>(1); //1: UI_TurretInfoPanel
                
                if(SystemManager.Instance.PanelManager.turretInfoPanel)//이미 InfoPanel이 존재할 경우 Reset해서 정보만 갱신
                    SystemManager.Instance.PanelManager.turretInfoPanel.GetComponent<UI_TurretInfoPanel>().Reset();
            }
            else
            {
                //UI_TurretInfoPanel 패널이 존재할 경우
                if (SystemManager.Instance.PanelManager.turretInfoPanel)
                {
                    //패널 비활성화
                    SystemManager.Instance.PanelManager.DisablePanel<UI_TurretInfoPanel>(SystemManager.Instance.PanelManager.turretInfoPanel.gameObject);
                }

                if (!SystemManager.Instance.PanelManager.turretInfoPanel)
                    SystemManager.Instance.PanelManager.EnablePanel<UI_TurretMngPanel>(0); //0: UI_TurretMngPanel
            }
        }    

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
}
