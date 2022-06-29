using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("speed")]
    //public float speed = 10.0F;
    //public float rotationSpeed = 100.0F;

    [SerializeField]
    Transform characterBody;

    [SerializeField]
    Transform cameraArm;

    //캐릭터 컨트롤러
    public float speed;      // 캐릭터 움직임 스피드.
    public float jumpSpeedF; // 캐릭터 점프 힘.
    public float gravity;    // 캐릭터에게 작용하는 중력.

    private CharacterController controller; // 현재 캐릭터가 가지고있는 캐릭터 컨트롤러 콜라이더.
    private Vector3 MoveDir;                // 캐릭터의 움직이는 방향.

    [SerializeField]
    bool isMove, isWalk, isRun;

    [SerializeField]
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        //캐릭터 컨트롤러
        speed = 6.0f;
        jumpSpeedF = 8.0f;
        gravity = 10.0f;

        MoveDir = Vector3.zero;
        controller = GetComponent<CharacterController>();
        animator = animator.GetComponent<Animator>();

        // 애니메이션 상태 변수 초기화
        isMove = false;
        isWalk = false;
        isRun = false;

    }

    // Update is called once per frame
    void Update()
    {
        LookAround();
    }
    public void UpdateMove(Vector3 inputDirection)
    {

        //캐릭터 컨트롤러
        // 현재 캐릭터가 땅에 있는가?
        if (controller.isGrounded)
        {
            // 앞, 뒤 움직임 셋팅. 
            // MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));            
            MoveDir = new Vector3(inputDirection.x, 0, inputDirection.y);
            
            // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다.
            MoveDir = transform.TransformDirection(MoveDir);

            // 스피드 증가.
            MoveDir *= speed;

        }

        // 캐릭터에 중력 적용.
        MoveDir.y -= gravity * Time.deltaTime;

        if (inputDirection != Vector3.zero)
        {                                   
            isMove = true;
            isWalk = true;
            isRun = false;
            //Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            //Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            //MoveDir = lookForward * MoveDir.y + lookRight * MoveDir.x;
        }
        else
        {                        
            isMove = false;
            isWalk = false;
            isRun = false;
        }

        animator.SetBool("isMove", isMove);
        animator.SetBool("isWalk", isWalk);
        animator.SetBool("isRun", isRun);

        // characterBody.forward = MoveDir;

        // 캐릭터 움직임.
        controller.Move(MoveDir * Time.deltaTime);
    }

    void LookAround()
    {
        Vector2 mouseDeltaPos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        float x = camAngle.x - mouseDeltaPos.y;

        // 회전 각도 제한
        if (x < 180f)
            x = Mathf.Clamp(x, -1f, 70f);
        else
            x = Mathf.Clamp(x, 335f, 361f);

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDeltaPos.x , camAngle.z);


    }
}
