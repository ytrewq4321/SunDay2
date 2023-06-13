using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider,
                                           rearLeftWheelCollider, rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform,
                                       rearLeftWheelTransform, rearRightWheelTransform;

    [SerializeField] private Transform centerMass;

    private PlayerInput inputActions;
    private float verticalInput, horizontalInput;
    private float currentSteerAngle;
    private float currentBreakForce;
    private bool isBreaking;
    private Rigidbody rb;

    private void OnEnable()
    {
        inputActions = GetComponent<PlayerInput>();
        inputActions.actions["Break"].started += ctx => OnBreakingStart(ctx);
        inputActions.actions["Break"].canceled += ctx => OnBreakingEnd(ctx);
    }

    private void OnDisable()
    {
        inputActions.actions["Break"].started -= ctx => OnBreakingStart(ctx);
        inputActions.actions["Break"].canceled -= ctx => OnBreakingEnd(ctx);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerMass.localPosition;
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void OnBreakingStart(InputAction.CallbackContext context)
    {
        isBreaking = context.started;
    }

    private void OnBreakingEnd(InputAction.CallbackContext context)
    {
        isBreaking = context.started;
    }

    private void GetInput()
    {
        var vertical = inputActions.actions["Move"].ReadValue<Vector2>().y;
        var horizontal = inputActions.actions["Move"].ReadValue<Vector2>().x;

        verticalInput = Mathf.Abs(vertical) > 0.4f ? vertical : 0;
        horizontalInput = Mathf.Abs(horizontal) > 0.4f ? horizontal : 0;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheels(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheels(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheels(rearLeftWheelCollider,rearLeftWheelTransform);
        UpdateSingleWheels(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheels(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
}
