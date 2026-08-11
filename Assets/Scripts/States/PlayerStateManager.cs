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
    public bool IsCrouching {get; private set;}
    private bool isSprinting;
    private Vector2 currentMoveInput;

    void Awake()
    {
        Instance = this;
    }
    void OnEnable()
    {
        InputManager.OnAim += AimInputs;
        InputManager.OnFire += FireInputs;
        InputManager.OnInteract += InteractInputs;
        InputManager.OnCrouch += CrouchInputs;
        InputManager.OnMove += MoveInputs;
        InputManager.OnSprint += SprintInputs;
    }
    void OnDisable()
    {
        InputManager.OnAim -= AimInputs;
        InputManager.OnFire -= FireInputs;
        InputManager.OnInteract -= InteractInputs;
        InputManager.OnCrouch -= CrouchInputs;
        InputManager.OnMove -= MoveInputs;
        InputManager.OnSprint -= SprintInputs;
    }

    void MoveInputs(Vector2 value)
    {
        currentMoveInput = value;
        UpdateMainStates();
    }
    void SprintInputs(bool value)
    {
        isSprinting = value;
        UpdateMainStates();
    }
    void UpdateMainStates()
    {
        if(currentMoveInput == Vector2.zero)
        {
            CurrentMainState = PlayerMainStates.Idle;
        }
        else if(isSprinting)
        {
            CurrentMainState = PlayerMainStates.Sprint;
        }
        else
        {
            CurrentMainState = PlayerMainStates.Walk;
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
    void CrouchInputs(bool value)
    {
        IsCrouching = value;
    }
}
