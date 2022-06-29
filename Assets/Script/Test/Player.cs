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

    //ĳ���� ��Ʈ�ѷ�
    public float speed;      // ĳ���� ������ ���ǵ�.
    public float jumpSpeedF; // ĳ���� ���� ��.
    public float gravity;    // ĳ���Ϳ��� �ۿ��ϴ� �߷�.

    private CharacterController controller; // ���� ĳ���Ͱ� �������ִ� ĳ���� ��Ʈ�ѷ� �ݶ��̴�.
    private Vector3 MoveDir;                // ĳ������ �����̴� ����.

    [SerializeField]
    bool isMove, isWalk, isRun;

    [SerializeField]
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        //ĳ���� ��Ʈ�ѷ�
        speed = 6.0f;
        jumpSpeedF = 8.0f;
        gravity = 10.0f;

        MoveDir = Vector3.zero;
        controller = GetComponent<CharacterController>();
        animator = animator.GetComponent<Animator>();

        // �ִϸ��̼� ���� ���� �ʱ�ȭ
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

        //ĳ���� ��Ʈ�ѷ�
        // ���� ĳ���Ͱ� ���� �ִ°�?
        if (controller.isGrounded)
        {
            // ��, �� ������ ����. 
            // MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));            
            MoveDir = new Vector3(inputDirection.x, 0, inputDirection.y);
            
            // ���͸� ���� ��ǥ�� ���ؿ��� ���� ��ǥ�� �������� ��ȯ�Ѵ�.
            MoveDir = transform.TransformDirection(MoveDir);

            // ���ǵ� ����.
            MoveDir *= speed;

        }

        // ĳ���Ϳ� �߷� ����.
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

        // ĳ���� ������.
        controller.Move(MoveDir * Time.deltaTime);
    }

    void LookAround()
    {
        Vector2 mouseDeltaPos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        float x = camAngle.x - mouseDeltaPos.y;

        // ȸ�� ���� ����
        if (x < 180f)
            x = Mathf.Clamp(x, -1f, 70f);
        else
            x = Mathf.Clamp(x, 335f, 361f);

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDeltaPos.x , camAngle.z);


    }
}
