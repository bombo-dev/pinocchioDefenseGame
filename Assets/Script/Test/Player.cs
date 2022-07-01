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

    //캐릭터 컨트롤러
    public float speed;      // 캐릭터 움직임 스피드.
    public float jumpSpeedF; // 캐릭터 점프 힘.
    public float gravity;    // 캐릭터에게 작용하는 중력.

    public CharacterController controller; // 현재 캐릭터가 가지고있는 캐릭터 컨트롤러 콜라이더.
    private Vector3 MoveDir;                // 캐릭터의 움직이는 방향.

    [SerializeField]
    public bool isMove; 
    bool isRun;

    [SerializeField]
    Animator animator;

    public float distance; // Run, Walk 애니메이션을 결정하기 위한 변수

    public Vector3 direction; // 조이스틱을 움직인 방향

    [SerializeField]
    Transform mainCamera;

    [SerializeField]
    Transform pino;

    Transform cameraArm;

    // StoryCameraMove storyCamMove;

    public bool isinJoystick = false;   // 조이스틱 내부의 입력인지 확인하는 플래그

    // Start is called before the first frame update
    void Start()
    {
        //캐릭터 컨트롤러
        speed = 10.0f;
        gravity = 20f;

        MoveDir = Vector3.zero;
        controller = GetComponent<CharacterController>();
        animator = animator.GetComponent<Animator>();
        cameraArm = transform.GetChild(1);
        // storyCamMove = mainCamera.GetComponent<StoryCameraMove>();

        // 애니메이션 상태 변수 초기화
        isMove = false;
        isRun = false;

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

            if (distance >= 90)
            {
                isRun = true;
                speed = 13f;

                //뛰기 효과음
                if (SoundEffectManager.Instance.loopEffectAudioSource.clip != SoundEffectManager.Instance.run)
                {
                    SoundEffectManager.Instance.loopEffectAudioSource.clip = SoundEffectManager.Instance.run;

                    //재생속도
                    SoundEffectManager.Instance.loopEffectAudioSource.pitch = 0.85f;

                    SoundEffectManager.Instance.loopEffectAudioSource.Play();
                }
            }
            else
            {
                isRun = false;
                speed = 8f;

                //걷기 효과음
                if (SoundEffectManager.Instance.loopEffectAudioSource.clip != SoundEffectManager.Instance.walk)
                {
                    SoundEffectManager.Instance.loopEffectAudioSource.clip = SoundEffectManager.Instance.walk;

                    //재생속도
                    SoundEffectManager.Instance.loopEffectAudioSource.pitch = 0.62f;

                    SoundEffectManager.Instance.loopEffectAudioSource.Play();
                }
            }
        }
        else
        {
            isMove = false;

            //기본
            if (SoundEffectManager.Instance.loopEffectAudioSource.clip != null)
                SoundEffectManager.Instance.loopEffectAudioSource.clip = null;
        }

        animator.SetBool("isMove", isMove);
        animator.SetBool("isRun", isRun);

        // 캐릭터 움직임.
         controller.Move(MoveDir * Time.deltaTime);


        // if (isinJoystick)
            RotationPlayer();

    }

    void RotationPlayer()
    {
        // 부모 해제
        mainCamera.SetParent(null);

        pino.eulerAngles = new Vector3(0, Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg, 0);

        // 부모 설정
        mainCamera.SetParent(transform.GetChild(1));

    }
}
