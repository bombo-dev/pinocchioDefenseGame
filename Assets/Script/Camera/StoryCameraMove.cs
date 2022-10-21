using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoryCameraMove : MonoBehaviour
{

    float zoomValue = 0;

    [SerializeField]
    Transform cameraArm;

    float preDistance, curDistance, moveDistance;   // 화면 줌을 위한 변수

    public bool isMouseButtonOver;    // 마우스(터치)가 UI 위에 있는 경우 

    public bool isMapClick;    // UI가 아닌 맵(게임 화면)을 클릭한 경우

    float zoomSpeed;

    public bool isCamRot = false;

    bool dontMove = false;      // 줌하는동안 한 손가락을 뗀 경우 카메라 이동 방지 플래그

    Player player;

    void Start()
    {
        player = cameraArm.parent.GetComponent<Player>();
    }
    
    void Update()
    {

        // 입력이 없을 때
        if (Input.touchCount == 0)
            dontMove = false;

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            //UpdateInputAtAnd();
            //UpdateAndFlag();
        }
        else
        {
            //UpdateInputAtWin();
            //UpdateWinFlag();
        }
    }

    #region Window

    void UpdateInputAtWin()
    {
        if (Input.GetMouseButton(0))
        {
            IsWinCamMove();
        }

        // 마우스 스크롤 입력이 들어오면
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            ZoomWinCam();
    }

    void IsWinCamMove()
    {
        // UI를 제외한 카메라 화면에서 입력이 들어오면
        if (!isMouseButtonOver && !EventSystem.current.IsPointerOverGameObject())
        {
            isMapClick = true;

            LookAround();


        }
        // 화면을 드래그하고 있는 상태에서 커서가 UI 안으로 들어가면
        else if (isMapClick && EventSystem.current.IsPointerOverGameObject())
        {
            LookAround();

            Debug.Log("클릭중");
        }
        // 화면을 드래그하고 있지 않은 상태에서 UI를 클릭하면
        else if (!isMapClick && EventSystem.current.IsPointerOverGameObject())
        {
            isMouseButtonOver = true;
        }

    }

    void ZoomWinCam()
    {
        zoomSpeed = 20.0f;

        // 줌 시킬 거리 구하기
        float moveDistance = Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;

        //  일정 값 이상 줌인, 줌아웃 하지 못하도록 설정
        if (Camera.main.fieldOfView - moveDistance < 20 || Camera.main.fieldOfView - moveDistance > 80)
        {
            float zoomValue = ControllZoom(moveDistance);
            Camera.main.fieldOfView = zoomValue;
        }
        else // 카메라 줌인
            Camera.main.fieldOfView -= Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;
    }





    void UpdateWinFlag()
    {
        if (!Input.GetMouseButton(0))
        {
            isMouseButtonOver = false;
            isMapClick = false;
        }
    }

    #endregion

    #region Android

    void UpdateInputAtAnd()
    {
        // 화면에 접촉된 손가락의 개수가 1개이면
        if (Input.touchCount == 1 && dontMove == false)
        {
            // MoveAndCam();

            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && !isMouseButtonOver)
            {
                isMapClick = true;

                player.isinJoystick = false;

                //LookAround();
            }
            else if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && isMapClick)
            {
                player.isinJoystick = false;

                //LookAround();
            }
            else if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && !isMapClick)
            {
                isMouseButtonOver = true;

                player.isinJoystick = true;
            }
            else
                return;

        }
        else if (Input.touchCount == 2 && !player.isMove)
        {
            ZoomAndCam();
        }
        else
            return;


    }
    void UpdateAndFlag()
    {
        if (Input.touchCount <= 0)
        {
            isMouseButtonOver = false;

            isMapClick = false;

            player.isinJoystick = false;
        }
    }

    /// <summary>
    /// 카메라 줌인, 줌아웃 : 하은비
    /// </summary>
    void ZoomAndCam()
    {

        Touch fstTouch = Input.GetTouch(0); // 첫 번째 터치 정보
        Touch scdTouch = Input.GetTouch(1); // 두 번째 터치 정보                        

        // 현재 화면에 접촉된 두 손가락 사이의 거리
        curDistance = (fstTouch.position - scdTouch.position).magnitude;

        // 화면에 처음 접촉했을 때의 두 손가락 사이의 거리
        preDistance = ((fstTouch.position - fstTouch.deltaPosition) - (scdTouch.position - scdTouch.deltaPosition)).magnitude;

        // 화면을 줌 시킬 크기 구하기
        moveDistance = curDistance - preDistance;

        //  일정 값 이상 줌인, 줌아웃 하지 못하도록 설정
        if (Camera.main.fieldOfView - 0.1f * moveDistance < 20 || Camera.main.fieldOfView - 0.1f * moveDistance > 80)
        {

            if (Input.touchCount == 2)
            {
                dontMove = true;
            }

            zoomValue = ControllZoom(moveDistance);
            Camera.main.fieldOfView = zoomValue;
        }
        else
        { // 카메라를 줌인, 줌아웃 
            if (Input.touchCount == 2)
            {
                dontMove = true;
            }
            Camera.main.fieldOfView -= 0.1f * moveDistance;
        }


    }



    #endregion


    /// <summary>
    /// 카메라 회전
    /// </summary>
    void LookAround()
    {
        isCamRot = true;

        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");

        Vector2 mouseDeltaPos = new Vector2(moveX, moveY);
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        // x축 기준 회전값 구하기
        float x = camAngle.x - mouseDeltaPos.y;

        // y축 기준 회전값 구하기
        float y = camAngle.y + mouseDeltaPos.x;

        // 회전 각도 제한        
        if (x < 180f)
            x = Mathf.Clamp(x, -1f, 70f);
        else
            x = Mathf.Clamp(x, 335f, 361f);

        cameraArm.rotation = Quaternion.Euler(x, y, 0);


    }

    /// <summary>
    /// 줌 크기 제한
    /// </summary>
    /// <param name="moveDist"></param>
    /// <returns></returns>
    float ControllZoom(float moveDist)
    {
        if (Camera.main.fieldOfView - moveDist < 20)
            zoomValue = 20;
        else if (Camera.main.fieldOfView - moveDist > 80)
            zoomValue = 80;
        else
            zoomValue = 0;

        return zoomValue;
    }
}
