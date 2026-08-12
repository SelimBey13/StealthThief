using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

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

    Vector2 moveInput;
    bool isMouseCrouching;
    bool isGamepadCrouching;
    bool isAiming;
    
    
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
    }
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        SetCameraDirection();
        SetPlayerMovement();
    }

    void OnEnable()
    {
        InputManager.OnMove += MoveInputs;
        InputManager.OnMouseCrouch += MouseCrouchInputs;
        InputManager.OnGamepadCrouch += GamepadCrouchInputs;
        InputManager.OnAim += AimInputs;
    }
    void OnDisable()
    {
        InputManager.OnMove -= MoveInputs;
        InputManager.OnMouseCrouch -= MouseCrouchInputs;
        InputManager.OnGamepadCrouch -= GamepadCrouchInputs;
        InputManager.OnAim -= AimInputs;
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
    Vector3 moveDirection = cameraDirectionForward * moveInput.y + cameraDirectionRight * moveInput.x;

   if(PlayerStateManager.Instance.CurrentMainState == PlayerStateManager.PlayerMainStates.Walk)
    {
        float currentSpeed = playerWalkSpeed;

        if (PlayerStateManager.Instance.IsMouseCrouching || PlayerStateManager.Instance.CrouchLocked)
        {
            currentSpeed = PlayerStateManager.Instance.IsAiming ? playerWalkSpeed / 4 : playerWalkSpeed / 2;
        }
   
        playerRigidbody.AddForce(moveDirection.normalized * currentSpeed, ForceMode.Force);
        Quaternion targetRotation = Quaternion.LookRotation(cameraDirectionForward);
        playerVisualTransform.rotation = Quaternion.Slerp(playerVisualTransform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);       
    }
    else if(PlayerStateManager.Instance.CurrentMainState == PlayerStateManager.PlayerMainStates.Sprint)
    {
        playerRigidbody.AddForce(moveDirection.normalized * playerSprintSpeed, ForceMode.Force);
        
        if(moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            playerVisualTransform.rotation = Quaternion.Slerp(playerVisualTransform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
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
    }
}
