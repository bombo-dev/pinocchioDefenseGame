using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColosseumCameraMove : MonoBehaviour
{    
    Transform cameraMove;   // 메인 카메라의 부모 오브젝트
    float zoomValue = 0;         // 줌 조정 값 


    [SerializeField]
    float zoomSpeed = 20.0f;  // 카메라 줌 속도

    [Header ("window")]

    float moveX, moveZ;        // 이동량

    [Header ("android")]

    public float touchSpeed = 5f;

    //유저정보 캐싱
    UserInfo userInfo;

    Vector2 curPos, prePos;
    Vector3 movePos;

    float preDistance, curDistance, moveDistance;   // 화면 줌을 위한 변수

    public bool isMouseButtonOver;     // 마우스(터치)가 UI 위에 있는 경우 

    public bool isMapClick;    // UI가 아닌 맵(게임 화면)을 클릭한 경우

    bool dontMove = false;    // 줌하는동안 한 손가락을 뗀 경우 카메라 이동 방지 플래그

    Vector3 initInputPos;

    bool isMove = true;

    // Start is called before the first frame update
    void Start()
    {        
        cameraMove = Camera.main.transform.parent;
        userInfo = SystemManager.Instance.UserInfo;       
    }

    // Update is called once per frame
    void Update()
    {
        // 입력이 없을 때
        if (Input.touchCount == 0)
            dontMove = false;

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            UpdateInputAtAnd();
            UpdateAndFlag();
        }
        else
        {
            UpdateInputAtWin();
            UpdateWinFlag();
        }

    }

    /// <summary>
    /// 일정한 값 이상으로 줌인, 줌아웃되지 않도록 조절 : 하은비
    /// </summary>
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

    #region Android

    /// <summary>
    /// 안드로이드에서 카메라 움직임 업데이트 : 하은비
    /// </summary>
    void UpdateInputAtAnd()
    {
        // 화면에 접촉된 손가락의 개수가 1개이면
        if (Input.touchCount == 1 && dontMove == false)
        {

            // 현재 입력 위치 저장
            initInputPos = Input.mousePosition;

            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && !isMouseButtonOver)
            {
                isMapClick = true;

                MoveAndCam();
            }
            else if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && isMapClick)
            {
                MoveAndCam();
            }
            else if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) && !isMapClick)
            {
                isMouseButtonOver = true;
            }
            else
                return;
            
        }
        else if (Input.touchCount == 2)
        {            
            ZoomAndCam();
        }
        else
            return;

        
    }

    /// <summary>
    /// 카메라 이동 : 하은비
    /// </summary>
    void MoveAndCam()
    {
        // 터치 상태 저장
        Touch touch = Input.GetTouch(0);

        float moveSpeed = userInfo.touchSpeed;

        // test.text = touch.phase.ToString();

        // 화면을 터치하는 순간 해당 위치 값 저장
        if (touch.phase == TouchPhase.Began)
        {    
            prePos = touch.position-touch.deltaPosition;
        }
        // 터치한 상태로 드래그하는 동안 화면 이동
        else if (touch.phase == TouchPhase.Moved)   
        {
            curPos = touch.position-touch.deltaPosition;

            // 이동할 방향 벡터를 구함 
            Vector2 dir = (prePos - curPos);

            // 배속에 따른 이동 속도 조절
            if (Time.timeScale == 1.0f)
            {
                moveSpeed = 5f;
                // Debug.Log("1배 speed = "+ Time.deltaTime * moveSpeed);
            }
            else if (Time.timeScale == 1.2f)
            {
                moveSpeed /= 1.2f;
                // Debug.Log("1.2배 speed ="+ Time.deltaTime * moveSpeed);
            }
            else if (Time.timeScale == 1.5f)
            {
                moveSpeed /= 1.5f;
                // Debug.Log("1.5배 speed =" + Time.deltaTime * moveSpeed);
            }
            else if (Time.timeScale == 2.0f)
            {
                moveSpeed /= 2.0f;
                // Debug.Log("2.0배 speed =" + Time.deltaTime * moveSpeed);
            }

                movePos = dir * Time.deltaTime * moveSpeed;

            if (cameraMove.position.x + movePos.x > 200)
            {
                cameraMove.position = new Vector3(200, cameraMove.position.y, cameraMove.position.z);
                movePos.x = 0;
            }
            else if (cameraMove.position.x + movePos.x < -200)
            {
                cameraMove.position = new Vector3(-200, cameraMove.position.y, cameraMove.position.z);
                movePos.x = 0;
            }
            if (cameraMove.position.z + movePos.y > 300)
            {
                cameraMove.position = new Vector3(cameraMove.position.x, cameraMove.position.y, 300);
                movePos.y = 0;
            }
            else if (cameraMove.position.z + movePos.y < -300)
            {
                cameraMove.position = new Vector3(cameraMove.position.x, cameraMove.position.y, -300);
                movePos.y = 0;
            }

            // 카메라 이동            
            cameraMove.Translate(movePos.x, 0, movePos.y);
            prePos = touch.position - touch.deltaPosition;
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

    void UpdateAndFlag()
    {
        if (Input.touchCount <= 0)
        {
            isMouseButtonOver = false;
            isMapClick = false;
        }
    }

    #endregion

  
    #region Window

    /// <summary>
    /// 카메라 움직임 업데이트 : 하은비
    /// </summary>
    void UpdateInputAtWin()
    {
        // 마우스 왼쪽 버튼을 클릭하는 동안
        IsWinCamMove();

        // 마우스 스크롤 입력이 들어오면
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            ZoomWinCam();
    }

    /// <summary>
    /// 카메라 이동 여부 검사 : 하은비
    /// </summary>
    void IsWinCamMove()
    {

        if (Input.GetMouseButtonDown(0))
            initInputPos = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            

            // UI를 제외한 카메라 화면에서 입력이 들어오면
            if (!isMouseButtonOver && !EventSystem.current.IsPointerOverGameObject())
            {
                isMapClick = true;

                MoveWinCam();
            }
            // 화면을 드래그하고 있는 상태에서 커서가 UI 안으로 들어가면
            else if (isMapClick && EventSystem.current.IsPointerOverGameObject())
            {
                MoveWinCam();
            }
            // 화면을 드래그하고 있지 않은 상태에서 UI를 클릭하면
            else if (!isMapClick && EventSystem.current.IsPointerOverGameObject())
            {
                isMouseButtonOver = true;
            }
        }
    }

    /// <summary>
    /// 카메라 이동 : 하은비
    /// </summary>
    void MoveWinCam()
    {

        float moveSpeed = userInfo.touchSpeed;       

        // 마우스 변위값 구하기 : 이동량
        moveX = Input.GetAxisRaw("Mouse X") * moveSpeed;
        moveZ = Input.GetAxisRaw("Mouse Y") * moveSpeed;

        // 이동 후 위치 구하기
         float amountX = cameraMove.position.x - moveX;
         float amountZ = cameraMove.position.z - moveZ;

        // 카메라 이동 영역 제한
        if (amountX > 200)
        {
            cameraMove.position = new Vector3(200, cameraMove.position.y, cameraMove.position.z);
            moveX = 0;
        }
        else if (amountX < -200)
        {
            cameraMove.position = new Vector3(-200, cameraMove.position.y, cameraMove.position.z);
            moveX = 0;
        }
        if (amountZ > 200)
        {
            cameraMove.position = new Vector3(cameraMove.position.x, cameraMove.position.y, 200);
            moveZ = 0;
        }
        else if (amountZ < -200)
        {
            cameraMove.position = new Vector3(cameraMove.position.x, cameraMove.position.y, -200);
            moveZ = 0;
        }

        /*
        // 카메라 이동 영역 제한
        if (amountX > 200)
        {
            moveX = (amountX - 200) - moveX;
        }
        else if (amountX < -200)
        {
            moveX = (amountX - (-200) + moveX);

        }


        if (amountZ > 300) 
        {
            moveZ = (amountZ - 300) - moveZ;
        }
        else if (amountZ < -300)
        {
            moveZ = (amountZ - (-300)) + moveZ;
        }
        */


        cameraMove.Translate(-moveX, 0, -moveZ);


        // 카메라 이동
        //cameraMove.position = new Vector3(cameraMove.position.x - moveX, 
        //                                                       cameraMove.position.y,  
        //                                                      cameraMove.position.z - moveZ);
    }

    /// <summary>
    /// 입력이 없으면 플래그 변수 초기화 : 하은비
    /// </summary>
    void UpdateWinFlag()
    {
        if (!Input.GetMouseButton(0))
        {
            isMouseButtonOver = false;
            isMapClick = false;
        }
    }

    /// <summary>
    /// 카메라 줌인, 줌아웃 : 하은비
    /// </summary>
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


    #endregion

}
