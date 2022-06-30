using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoryCameraMove : MonoBehaviour
{

    float zoomValue = 0;

    [SerializeField]
    Transform cameraArm;

    float preDistance, curDistance, moveDistance;   // ȭ�� ���� ���� ����

    public bool isMouseButtonOver;    // ���콺(��ġ)�� UI ���� �ִ� ��� 

    public bool isMapClick;    // UI�� �ƴ� ��(���� ȭ��)�� Ŭ���� ���

    float zoomSpeed;

    public bool isCamRot = false;

    bool dontMove = false;      // ���ϴµ��� �� �հ����� �� ��� ī�޶� �̵� ���� �÷���

    Player player;

    void Start()
    {
        player = cameraArm.parent.GetComponent<Player>();
    }
    
    void Update()
    {

        // �Է��� ���� ��
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

        // ���콺 ��ũ�� �Է��� ������
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            ZoomWinCam();
    }

    void IsWinCamMove()
    {
        // UI�� ������ ī�޶� ȭ�鿡�� �Է��� ������
        if (!isMouseButtonOver && !EventSystem.current.IsPointerOverGameObject())
        {
            isMapClick = true;

            LookAround();


        }
        // ȭ���� �巡���ϰ� �ִ� ���¿��� Ŀ���� UI ������ ����
        else if (isMapClick && EventSystem.current.IsPointerOverGameObject())
        {
            LookAround();

            Debug.Log("Ŭ����");
        }
        // ȭ���� �巡���ϰ� ���� ���� ���¿��� UI�� Ŭ���ϸ�
        else if (!isMapClick && EventSystem.current.IsPointerOverGameObject())
        {
            isMouseButtonOver = true;
        }

    }

    void ZoomWinCam()
    {
        zoomSpeed = 20.0f;

        // �� ��ų �Ÿ� ���ϱ�
        float moveDistance = Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;

        //  ���� �� �̻� ����, �ܾƿ� ���� ���ϵ��� ����
        if (Camera.main.fieldOfView - moveDistance < 20 || Camera.main.fieldOfView - moveDistance > 80)
        {
            float zoomValue = ControllZoom(moveDistance);
            Camera.main.fieldOfView = zoomValue;
        }
        else // ī�޶� ����
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
        // ȭ�鿡 ���˵� �հ����� ������ 1���̸�
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
    /// ī�޶� ����, �ܾƿ� : ������
    /// </summary>
    void ZoomAndCam()
    {

        Touch fstTouch = Input.GetTouch(0); // ù ��° ��ġ ����
        Touch scdTouch = Input.GetTouch(1); // �� ��° ��ġ ����                        

        // ���� ȭ�鿡 ���˵� �� �հ��� ������ �Ÿ�
        curDistance = (fstTouch.position - scdTouch.position).magnitude;

        // ȭ�鿡 ó�� �������� ���� �� �հ��� ������ �Ÿ�
        preDistance = ((fstTouch.position - fstTouch.deltaPosition) - (scdTouch.position - scdTouch.deltaPosition)).magnitude;

        // ȭ���� �� ��ų ũ�� ���ϱ�
        moveDistance = curDistance - preDistance;

        //  ���� �� �̻� ����, �ܾƿ� ���� ���ϵ��� ����
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
        { // ī�޶� ����, �ܾƿ� 
            if (Input.touchCount == 2)
            {
                dontMove = true;
            }
            Camera.main.fieldOfView -= 0.1f * moveDistance;
        }


    }



    #endregion


    /// <summary>
    /// ī�޶� ȸ��
    /// </summary>
    void LookAround()
    {
        isCamRot = true;

        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");

        Vector2 mouseDeltaPos = new Vector2(moveX, moveY);
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        // x�� ���� ȸ���� ���ϱ�
        float x = camAngle.x - mouseDeltaPos.y;

        // y�� ���� ȸ���� ���ϱ�
        float y = camAngle.y + mouseDeltaPos.x;

        // ȸ�� ���� ����        
        if (x < 180f)
            x = Mathf.Clamp(x, -1f, 70f);
        else
            x = Mathf.Clamp(x, 335f, 361f);

        cameraArm.rotation = Quaternion.Euler(x, y, 0);


    }

    /// <summary>
    /// �� ũ�� ����
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
