using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColosseumCameraMove : MonoBehaviour
{    
    Transform cameraMove;   // 메인 카메라의 부모 오브젝트
    float zoomValue = 0;         // 줌 조정 값 

    [Header ("window")]

    [SerializeField]
    float moveSpeed = 2.8f;    // 카메라 이동 속도

    [SerializeField]
    float zoomSpeed = 20.0f;  // 카메라 줌 속도

    float moveX, moveZ;        // 이동량

    [Header ("android")]

    [SerializeField]
    float touchSpeed = 0.01f;

    Vector2 curPos, prePos;
    Vector3 movePos;

    float preDistance, curDistance, moveDistance;

    Vector3 fixedPos;

    bool isMouseButtonOver;

    bool isMapClick;

    // Start is called before the first frame update
    void Start()
    {        
        cameraMove = Camera.main.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        isCheckPointerOverGameObject();
        UpdateInputAtAnd();
        UpdateInputAtWin();
    }

    /// <summary>
    /// 일정한 값 이상으로 줌인, 줌아웃되지 않도록 조절 : 하은비
    /// </summary>
    bool ControllZoom()
    {
        if (Camera.main.fieldOfView < 20 || Camera.main.fieldOfView > 80)
        {
            if (Camera.main.fieldOfView < 20)
                zoomValue = 20;
            else
                zoomValue = 80;
            return true;
        }
        return false;
    }

    #region Android

    /// <summary>
    /// 안드로이드에서 카메라 움직임 업데이트 : 하은비
    /// </summary>
    void UpdateInputAtAnd()
    {
        // 화면에 접촉된 손가락의 개수가 1개이면
        if (Input.touchCount == 1 && !EventSystem.current.IsPointerOverGameObject())
            MoveAndCam();
        else if (Input.touchCount == 2)
            ZoomAndCam();
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

        // 화면을 터치하는 순간 해당 위치 값 저장
        if (touch.phase == TouchPhase.Began)
        {    
            prePos = touch.position;
        }
        // 터치한 상태로 드래그하는 동안 화면 이동
        else if (touch.phase == TouchPhase.Moved)   
        {
            curPos = touch.position;
            // 이동할 방향 벡터를 구함
            Vector3 dir = (prePos - curPos).normalized;            
            movePos = dir * Time.deltaTime * touchSpeed;

            // 카메라 이동
            cameraMove.Translate(movePos);
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
        preDistance = ((fstTouch.position - fstTouch.deltaPosition) - (scdTouch.position- scdTouch.deltaPosition)).magnitude;
        
        // 화면을 줌 시킬 크기 구하기
        moveDistance = curDistance - preDistance;

        //  일정 값 이상 줌인, 줌아웃 하지 못하도록 설정
        if (ControllZoom())
            Camera.main.fieldOfView = zoomValue;

        //  카메라를 줌인, 줌아웃

            Camera.main.fieldOfView -= 0.3f * moveDistance;
    }
    

    #endregion

    #region Window

    /// <summary>
    /// 카메라 움직임 업데이트 : 하은비
    /// </summary>
    void UpdateInputAtWin()
    {
        MoveWinCam();

        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            ZoomWinCam();
    }

    /// <summary>
    /// 카메라 이동 : 하은비
    /// </summary>
    void MoveWinCam()
    {
        // 마우스 왼쪽 버튼 드래그 처리
        if (Input.GetMouseButton(0))
        {
            
            if (!isMouseButtonOver && !EventSystem.current.IsPointerOverGameObject())
            {
                isMapClick = true;

                // 마우스 변위값 구하기
                moveX = Input.GetAxisRaw("Mouse X") * moveSpeed;
                moveZ = Input.GetAxisRaw("Mouse Y") * moveSpeed;

                // 카메라 이동
                cameraMove.Translate(-moveX, 0, -moveZ);
            }
            else if(isMapClick && EventSystem.current.IsPointerOverGameObject())
            {
                // 마우스 변위값 구하기
                moveX = Input.GetAxisRaw("Mouse X") * moveSpeed;
                moveZ = Input.GetAxisRaw("Mouse Y") * moveSpeed;

                // 카메라 이동
                cameraMove.Translate(-moveX, 0, -moveZ);
            }
            else if(!isMapClick && EventSystem.current.IsPointerOverGameObject())
            {
                isMouseButtonOver = true;
            }
        }
    }

    void isCheckPointerOverGameObject()
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
        //  일정 값 이상 줌인, 줌아웃 하지 못하도록 설정
        if(ControllZoom())
            Camera.main.fieldOfView = zoomValue;

        // 카메라 줌인
        Camera.main.fieldOfView -= Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;        
    }


    #endregion
}
