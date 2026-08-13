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
    private bool sprintLocked;
    public bool CrouchLocked {get; private set;}

    void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        Debug.Log(CurrentMainState);
        Debug.Log("SprintLock"+sprintLocked);
        Debug.Log("CrouchLock"+CrouchLocked);
        Debug.Log("Crouch: " + IsMouseCrouching);
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
            sprintLocked = false;
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
            else if(sprintLocked && !CrouchLocked)
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
            
            CrouchLocked = !CrouchLocked;
            UpdateMainStates();  
        }

    }
    void MouseSprintInputs(bool value)
    {
        IsMouseSprinting = value;
        UpdateMainStates();
    }

    void GamepadSprintInputs(bool value)
    {
        IsGamepadSprinting = value;

        if (value && CurrentMainState == PlayerMainStates.Walk)
        {
            sprintLocked = true;
            CrouchLocked = false;
            UpdateMainStates();
        }

    }
}
