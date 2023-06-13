using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraMain;
    [SerializeField] InputManager inputManager;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSmoothTime=0.1f ;
    [SerializeField] private float jumpTimeout=0.5f;
    [SerializeField] private float fallTimeout = 0.15f;

    private CharacterController controller;
    private Animator animator;
    private Vector3 playerVelocity;
    private Vector3 moveDir;

    private float rotationVelocity;
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    private bool grounded;
    private bool isRunning;
    private bool isShooting;

    private int animSpeedID;
    private int animJumpID;
    private int animFallID;
    private int animGroundedID;
    private int animShootID;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraMain = Camera.main.transform;

        animator = GetComponent<Animator>();
        animSpeedID = Animator.StringToHash("Speed");
        animJumpID = Animator.StringToHash("Jump");
        animFallID = Animator.StringToHash("Fall");
        animGroundedID = Animator.StringToHash("Grounded");
        animShootID = Animator.StringToHash("Shoot");

        jumpTimeoutDelta = jumpTimeout;
        fallTimeoutDelta = fallTimeout;
    }
    void Update()
    {
        grounded = controller.isGrounded;
        animator.SetBool(animGroundedID, grounded);

        GetInput();
        Move();
        Shoot();
        Jump();
    }

    private bool GetJumpInput()
    {
        return inputManager.Jump || Input.GetKeyDown(KeyCode.Space);
    }

    private void GetMoveInput()
    {
        moveDir = cameraMain.right * inputManager.Move.x + cameraMain.forward * inputManager.Move.y;
        moveDir.y = 0;
    }

    private void GetRunInput()
    {
        isRunning = inputManager.Run;
    }

    private void GetShootInput()
    {
        isShooting = inputManager.Shoot;
    }

    private void GetInput()
    {
        GetRunInput();
        GetMoveInput();
        GetShootInput();
    }

    private void Shoot()
    {
        animator.SetBool(animShootID, isShooting);
    }
    private void Move()
    {
        var speed = isRunning ? runSpeed : walkSpeed;
        var move = moveDir * speed;
        controller.Move(move * Time.deltaTime);
        animator.SetFloat(animSpeedID, move.sqrMagnitude);

        if (move != Vector3.zero)
        {
            var targetRotation = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }

    private void Jump()
    {
        
        if (grounded)
        {
            animator.SetBool(animJumpID, false);
            animator.SetBool(animFallID, false);
            

            if (playerVelocity.y<0f)
            {
                playerVelocity.y = -2f;
            }

            if(GetJumpInput() && jumpTimeoutDelta<=0)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravityValue);
                animator.SetBool(animJumpID, true);

            }

            if(jumpTimeoutDelta>=0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            jumpTimeoutDelta = jumpTimeout;

            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;  
            }
            else
            {
                animator.SetBool(animFallID, true);
            }
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
