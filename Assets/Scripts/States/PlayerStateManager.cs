using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public enum PlayerMainStates
    {
        Idle,
        Walk,
        Sprint,
    }
    public static PlayerStateManager Instance {get; private set;}
    public PlayerMainStates CurrentMainState { get; private set; }

    public bool IsAiming {get; private set;}
    public bool IsFiring {get; private set;}
    public bool IsInteracting {get; private set;}
    public bool IsMouseCrouching {get; private set;}
    public bool IsGamepadCrouching {get; private set;}
    public bool IsMouseSprinting {get; private set;}
    public bool IsGamepadSprinting {get; private set;}


    private Vector2 currentMoveInput;
    public bool SprintLocked {get; private set;}
    public bool CrouchLocked {get; private set;}

    void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        Debug.Log(CurrentMainState);
        Debug.Log("SprintLock"+SprintLocked);
        Debug.Log("CrouchLock"+CrouchLocked);
        Debug.Log("Crouch: " + IsMouseCrouching);
        Debug.Log("Aiming: " + IsAiming);
    }

    void OnEnable()
    {
        InputManager.OnAim += AimInputs;
        InputManager.OnFire += FireInputs;
        InputManager.OnInteract += InteractInputs;
        InputManager.OnMove += MoveInputs;

        InputManager.OnMouseCrouch += MouseCrouchInputs;
        InputManager.OnGamepadCrouch += GamepadCrouchInputs;

        InputManager.OnMouseSprint += MouseSprintInputs;
        InputManager.OnGamepadSprint += GamepadSprintInputs;
    }
    void OnDisable()
    {
       InputManager.OnAim -= AimInputs;
        InputManager.OnFire -= FireInputs;
        InputManager.OnInteract -= InteractInputs;
        InputManager.OnMove -= MoveInputs;

        InputManager.OnMouseCrouch -= MouseCrouchInputs;
        InputManager.OnGamepadCrouch -= GamepadCrouchInputs;

        InputManager.OnMouseSprint -= MouseSprintInputs;
        InputManager.OnGamepadSprint -= GamepadSprintInputs;
    }

    void MoveInputs(Vector2 value)
    {
        currentMoveInput = value;

         if (currentMoveInput == Vector2.zero)
         {
            SprintLocked = false;
         }
        
        UpdateMainStates();
    }
    void UpdateMainStates()
    {
        if(InputManager.CurrentActiveDevice == ActiveDevice.Gamepad)
        {
            if(currentMoveInput == Vector2.zero)
            {
                CurrentMainState = PlayerMainStates.Idle;
            }
            else if(SprintLocked && !CrouchLocked)
            {
                CurrentMainState = PlayerMainStates.Sprint;
            }
            else
            {
                CurrentMainState = PlayerMainStates.Walk;
            }  
            }
        else if(InputManager.CurrentActiveDevice == ActiveDevice.Mouse)
        {
            if(currentMoveInput == Vector2.zero)
            {
                CurrentMainState = PlayerMainStates.Idle;
            }
            else if(IsMouseSprinting && !IsMouseCrouching)
            {
                CurrentMainState = PlayerMainStates.Sprint;
            }
            else
            {
                CurrentMainState = PlayerMainStates.Walk;
            } 
        }
        
    }
    void AimInputs(bool value)
    {
        if (value && CurrentMainState == PlayerMainStates.Sprint)
        {
            SprintLocked = false;
            IsMouseSprinting = false;
            UpdateMainStates();
        }
        IsAiming = value;
    }
    void FireInputs(bool value)
    {
        IsFiring = value;
    }
    void InteractInputs(bool value)
    {
        IsInteracting = value;
    }
    void MouseCrouchInputs(bool value)
    {
        IsMouseCrouching = value;

    }
    void GamepadCrouchInputs(bool value)
    {
        IsGamepadCrouching = value;

        if(value)
        {
            SprintLocked = false;
            CrouchLocked = !CrouchLocked;
            UpdateMainStates();  
        }

    }
    void MouseSprintInputs(bool value)
    {
        //if(IsAiming) return; ->
        IsMouseSprinting = value;
        if(value) IsAiming = false;
        UpdateMainStates();
    }

    void GamepadSprintInputs(bool value)
    {
        IsGamepadSprinting = value;

        if (value && CurrentMainState == PlayerMainStates.Walk)
        {
            SprintLocked = true;
            CrouchLocked = false;
            IsAiming = false;
            UpdateMainStates();
        }

    }
}
