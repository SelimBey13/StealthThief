using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody playerRigidbody;
    [SerializeField] Transform playerVisualTransform;
    
    [Header("Movement")]
    [SerializeField] float playerWalkSpeed;
    [SerializeField] float playerSprintSpeed;
    [SerializeField] float groundDrag;
    [SerializeField] float rotationSpeed;
    Vector3 cameraDirectionForward;
    Vector3 cameraDirectionRight;
    Vector3 moveDirection;
    [SerializeField] float aimGamepadTurnSensitivity;
    [SerializeField] float aimMouseTurnSensitivity;
    private float aimTurnSensitivity;
    private ActiveDevice activeDevice;

    Vector2 moveInput;
    bool isMouseCrouching;
    bool isGamepadCrouching;
    bool isAiming;
    Vector2 generalLookInput;
    
    
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
        activeDevice = InputManager.CurrentActiveDevice;
    }

    void Start()
    {
        playerRigidbody.linearDamping = groundDrag;
    }
    void Update()
    {
        SetCameraDirection();
        UpdateVisualRotation();
    }
    void FixedUpdate()
    {
        
        moveDirection = cameraDirectionForward * moveInput.y + cameraDirectionRight * moveInput.x;
        SetPlayerMovement();
    }

    void OnEnable()
    {
        InputManager.OnMove += MoveInputs;
        InputManager.OnMouseCrouch += MouseCrouchInputs;
        InputManager.OnGamepadCrouch += GamepadCrouchInputs;
        InputManager.OnAim += AimInputs;
        InputManager.OnMouseLook += MouseLookInputs;
        InputManager.OnGamepadLook += GamepadLookInputs;
        InputManager.OnActiveDeviceChanged += ActiveDeviceInfo;

    }
    void OnDisable()
    {
        InputManager.OnMove -= MoveInputs;
        InputManager.OnMouseCrouch -= MouseCrouchInputs;
        InputManager.OnGamepadCrouch -= GamepadCrouchInputs;
        InputManager.OnAim -= AimInputs;
        InputManager.OnMouseLook -= MouseLookInputs;
        InputManager.OnGamepadLook -= GamepadLookInputs;
    }

    void SetCameraDirection()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraDirectionForward = new Vector3(cameraForward.x,0f,cameraForward.z).normalized;
        cameraDirectionRight = new Vector3(cameraRight.x,0f,cameraRight.z).normalized;
    }
    void SetPlayerMovement()
{

  if(PlayerStateManager.Instance.CurrentMainState == PlayerStateManager.PlayerMainStates.Walk)
    { 
        float currentSpeed = playerWalkSpeed;

        if (PlayerStateManager.Instance.IsMouseCrouching || PlayerStateManager.Instance.CrouchLocked)
        {
            currentSpeed = PlayerStateManager.Instance.IsAiming ? playerWalkSpeed * 0.80f : playerWalkSpeed * 0.90f;
        }
        playerRigidbody.AddForce(moveDirection.normalized * currentSpeed, ForceMode.Force);
    }
    else if(PlayerStateManager.Instance.CurrentMainState == PlayerStateManager.PlayerMainStates.Sprint)
    {
        playerRigidbody.AddForce(moveDirection.normalized * playerSprintSpeed, ForceMode.Force);
    }
}

    void UpdateVisualRotation()
{
    Quaternion targetRotation;

    aimTurnSensitivity = (activeDevice == ActiveDevice.Mouse)? aimMouseTurnSensitivity : aimGamepadTurnSensitivity;

    if(PlayerStateManager.Instance.IsAiming)
    {
        playerVisualTransform.Rotate(Vector3.up,generalLookInput.x * aimTurnSensitivity * Time.deltaTime);
        return;

    }
    else if (PlayerStateManager.Instance.CurrentMainState == PlayerStateManager.PlayerMainStates.Walk)
    {
        targetRotation = Quaternion.LookRotation(cameraDirectionForward);
    }
    else if (PlayerStateManager.Instance.CurrentMainState == PlayerStateManager.PlayerMainStates.Sprint && moveDirection != Vector3.zero)
    {
        targetRotation = Quaternion.LookRotation(moveDirection);
    }
    else
    {
        return;
    }

    playerVisualTransform.rotation = Quaternion.Slerp(playerVisualTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
}
    
    
    
    void MoveInputs(Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }
    void MouseCrouchInputs(bool value)
    {
        isMouseCrouching = value;
    }
    void GamepadCrouchInputs(bool value)
    {
        isGamepadCrouching = value;
    }
    void AimInputs(bool value)
    {
        isAiming = value;
        if (value && WeaponDirector.Instance.Index == 0 && WeaponDirector.Instance.weaponSelected)
        {
            playerVisualTransform.forward = cameraDirectionForward;
        }
    }
    void MouseLookInputs(Vector2 mouseLookInput)
    {
        generalLookInput = mouseLookInput;
    }
    void GamepadLookInputs(Vector2 gamepadLookInput)
    {
        generalLookInput = gamepadLookInput;
    }
    void ActiveDeviceInfo(ActiveDevice activeDevice)
    {
        this.activeDevice = activeDevice;
    }
}
