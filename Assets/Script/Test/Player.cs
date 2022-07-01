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

    //ĳ���� ��Ʈ�ѷ�
    public float speed;      // ĳ���� ������ ���ǵ�.
    public float jumpSpeedF; // ĳ���� ���� ��.
    public float gravity;    // ĳ���Ϳ��� �ۿ��ϴ� �߷�.

    public CharacterController controller; // ���� ĳ���Ͱ� �������ִ� ĳ���� ��Ʈ�ѷ� �ݶ��̴�.
    private Vector3 MoveDir;                // ĳ������ �����̴� ����.

    [SerializeField]
    public bool isMove; 
    bool isRun;

    [SerializeField]
    Animator animator;

    public float distance; // Run, Walk �ִϸ��̼��� �����ϱ� ���� ����

    public Vector3 direction; // ���̽�ƽ�� ������ ����

    [SerializeField]
    Transform mainCamera;

    [SerializeField]
    Transform pino;

    Transform cameraArm;

    // StoryCameraMove storyCamMove;

    public bool isinJoystick = false;   // ���̽�ƽ ������ �Է����� Ȯ���ϴ� �÷���

    // Start is called before the first frame update
    void Start()
    {
        //ĳ���� ��Ʈ�ѷ�
        speed = 10.0f;
        gravity = 20f;

        MoveDir = Vector3.zero;
        controller = GetComponent<CharacterController>();
        animator = animator.GetComponent<Animator>();
        cameraArm = transform.GetChild(1);
        // storyCamMove = mainCamera.GetComponent<StoryCameraMove>();

        // �ִϸ��̼� ���� ���� �ʱ�ȭ
        isMove = false;
        isRun = false;

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

            if (distance >= 90)
            {
                isRun = true;
                speed = 13f;

                //�ٱ� ȿ����
                if (SoundEffectManager.Instance.loopEffectAudioSource.clip != SoundEffectManager.Instance.run)
                {
                    SoundEffectManager.Instance.loopEffectAudioSource.clip = SoundEffectManager.Instance.run;

                    //����ӵ�
                    SoundEffectManager.Instance.loopEffectAudioSource.pitch = 0.85f;

                    SoundEffectManager.Instance.loopEffectAudioSource.Play();
                }
            }
            else
            {
                isRun = false;
                speed = 8f;

                //�ȱ� ȿ����
                if (SoundEffectManager.Instance.loopEffectAudioSource.clip != SoundEffectManager.Instance.walk)
                {
                    SoundEffectManager.Instance.loopEffectAudioSource.clip = SoundEffectManager.Instance.walk;

                    //����ӵ�
                    SoundEffectManager.Instance.loopEffectAudioSource.pitch = 0.62f;

                    SoundEffectManager.Instance.loopEffectAudioSource.Play();
                }
            }
        }
        else
        {
            isMove = false;

            //�⺻
            if (SoundEffectManager.Instance.loopEffectAudioSource.clip != null)
                SoundEffectManager.Instance.loopEffectAudioSource.clip = null;
        }

        animator.SetBool("isMove", isMove);
        animator.SetBool("isRun", isRun);

        // ĳ���� ������.
         controller.Move(MoveDir * Time.deltaTime);


        // if (isinJoystick)
            RotationPlayer();

    }

    void RotationPlayer()
    {
        // �θ� ����
        mainCamera.SetParent(null);

        pino.eulerAngles = new Vector3(0, Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg, 0);

        // �θ� ����
        mainCamera.SetParent(transform.GetChild(1));

    }
}
