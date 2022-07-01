using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColosseumCameraMove : MonoBehaviour
{    
    Transform cameraMove;   // ���� ī�޶��� �θ� ������Ʈ
    float zoomValue = 0;         // �� ���� �� 


    [SerializeField]
    float zoomSpeed = 20.0f;  // ī�޶� �� �ӵ�

    [Header ("window")]

    float moveX, moveZ;        // �̵���

    [Header ("android")]

    public float touchSpeed = 5f;

    //�������� ĳ��
    UserInfo userInfo;

    Vector2 curPos, prePos;
    Vector3 movePos;

    float preDistance, curDistance, moveDistance;   // ȭ�� ���� ���� ����

    public bool isMouseButtonOver;     // ���콺(��ġ)�� UI ���� �ִ� ��� 

    public bool isMapClick;    // UI�� �ƴ� ��(���� ȭ��)�� Ŭ���� ���

    bool dontMove = false;    // ���ϴµ��� �� �հ����� �� ��� ī�޶� �̵� ���� �÷���

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
        // �Է��� ���� ��
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
    /// ������ �� �̻����� ����, �ܾƿ����� �ʵ��� ���� : ������
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
    /// �ȵ���̵忡�� ī�޶� ������ ������Ʈ : ������
    /// </summary>
    void UpdateInputAtAnd()
    {
        // ȭ�鿡 ���˵� �հ����� ������ 1���̸�
        if (Input.touchCount == 1 && dontMove == false)
        {

            // ���� �Է� ��ġ ����
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
    /// ī�޶� �̵� : ������
    /// </summary>
    void MoveAndCam()
    {
        // ��ġ ���� ����
        Touch touch = Input.GetTouch(0);

        float moveSpeed = userInfo.touchSpeed;

        // test.text = touch.phase.ToString();

        // ȭ���� ��ġ�ϴ� ���� �ش� ��ġ �� ����
        if (touch.phase == TouchPhase.Began)
        {    
            prePos = touch.position-touch.deltaPosition;
        }
        // ��ġ�� ���·� �巡���ϴ� ���� ȭ�� �̵�
        else if (touch.phase == TouchPhase.Moved)   
        {
            curPos = touch.position-touch.deltaPosition;

            // �̵��� ���� ���͸� ���� 
            Vector2 dir = (prePos - curPos);

            // ��ӿ� ���� �̵� �ӵ� ����
            if (Time.timeScale == 1.0f)
            {
                moveSpeed = 5f;
                // Debug.Log("1�� speed = "+ Time.deltaTime * moveSpeed);
            }
            else if (Time.timeScale == 1.2f)
            {
                moveSpeed /= 1.2f;
                // Debug.Log("1.2�� speed ="+ Time.deltaTime * moveSpeed);
            }
            else if (Time.timeScale == 1.5f)
            {
                moveSpeed /= 1.5f;
                // Debug.Log("1.5�� speed =" + Time.deltaTime * moveSpeed);
            }
            else if (Time.timeScale == 2.0f)
            {
                moveSpeed /= 2.0f;
                // Debug.Log("2.0�� speed =" + Time.deltaTime * moveSpeed);
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

            // ī�޶� �̵�            
            cameraMove.Translate(movePos.x, 0, movePos.y);
            prePos = touch.position - touch.deltaPosition;
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
    /// ī�޶� ������ ������Ʈ : ������
    /// </summary>
    void UpdateInputAtWin()
    {
        // ���콺 ���� ��ư�� Ŭ���ϴ� ����
        IsWinCamMove();

        // ���콺 ��ũ�� �Է��� ������
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            ZoomWinCam();
    }

    /// <summary>
    /// ī�޶� �̵� ���� �˻� : ������
    /// </summary>
    void IsWinCamMove()
    {

        if (Input.GetMouseButtonDown(0))
            initInputPos = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            

            // UI�� ������ ī�޶� ȭ�鿡�� �Է��� ������
            if (!isMouseButtonOver && !EventSystem.current.IsPointerOverGameObject())
            {
                isMapClick = true;

                MoveWinCam();
            }
            // ȭ���� �巡���ϰ� �ִ� ���¿��� Ŀ���� UI ������ ����
            else if (isMapClick && EventSystem.current.IsPointerOverGameObject())
            {
                MoveWinCam();
            }
            // ȭ���� �巡���ϰ� ���� ���� ���¿��� UI�� Ŭ���ϸ�
            else if (!isMapClick && EventSystem.current.IsPointerOverGameObject())
            {
                isMouseButtonOver = true;
            }
        }
    }

    /// <summary>
    /// ī�޶� �̵� : ������
    /// </summary>
    void MoveWinCam()
    {

        float moveSpeed = userInfo.touchSpeed;       

        // ���콺 ������ ���ϱ� : �̵���
        moveX = Input.GetAxisRaw("Mouse X") * moveSpeed;
        moveZ = Input.GetAxisRaw("Mouse Y") * moveSpeed;

        // �̵� �� ��ġ ���ϱ�
         float amountX = cameraMove.position.x - moveX;
         float amountZ = cameraMove.position.z - moveZ;

        // ī�޶� �̵� ���� ����
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
        // ī�޶� �̵� ���� ����
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


        // ī�޶� �̵�
        //cameraMove.position = new Vector3(cameraMove.position.x - moveX, 
        //                                                       cameraMove.position.y,  
        //                                                      cameraMove.position.z - moveZ);
    }

    /// <summary>
    /// �Է��� ������ �÷��� ���� �ʱ�ȭ : ������
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
    /// ī�޶� ����, �ܾƿ� : ������
    /// </summary>
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


    #endregion

}
