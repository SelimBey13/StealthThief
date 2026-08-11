using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {get; private set;}
    private InputActions inputActions;
    public static event Action<Vector2> OnMove;
    public static event Action<bool> OnSprint;
    public static event Action<bool> OnCrouch;
    public static event Action<bool> OnInteract;
    public static event Action<bool> OnFire;
    public static event Action<bool> OnAim;
    public static event Action<Vector2> OnMouseLook;
    public static event Action<Vector2> OnGamepadLook;
    public static ActiveDevice CurrentActiveDevice { get; private set; } = ActiveDevice.Mouse;
    public static event Action<ActiveDevice> OnActiveDeviceChanged;

    void Awake()
    {
        AwakingController();
    }

    void OnEnable()
    {
        inputActions.Player.Enable(); // bu sistem daha sonra değiştirilecek,menü mantıgı

        inputActions.Player.Move.performed += MovePerformed;
        inputActions.Player.Move.canceled += MoveCanceled;

        inputActions.Player.Sprint.performed += SprintPerformed;
        inputActions.Player.Sprint.canceled += SprintCanceled;
        
        inputActions.Player.Crouch.performed += CrouchPerformed;
        inputActions.Player.Crouch.canceled += CrouchCanceled;

        inputActions.Player.Interact.performed += InteractPerformed;
        inputActions.Player.Interact.canceled += InteractCanceled;

        inputActions.Player.Fire.performed += FirePerformed;
        inputActions.Player.Fire.canceled += FireCanceled;

        inputActions.Player.Aim.performed += AimPerformed;
        inputActions.Player.Aim.canceled += AimCanceled;
        
        inputActions.Player.Look.performed += LookPerformed;
        inputActions.Player.Look.canceled += LookCanceled;
    }

    void OnDisable()
    {
        inputActions.Player.Disable(); // bu sistem daha sonra değiştirilecek,menü mantıgı

        inputActions.Player.Move.performed -= MovePerformed;
        inputActions.Player.Move.canceled -= MoveCanceled;

        inputActions.Player.Sprint.performed -= SprintPerformed;
        inputActions.Player.Sprint.canceled -= SprintCanceled;

        inputActions.Player.Crouch.performed -= CrouchPerformed;
        inputActions.Player.Crouch.canceled -= CrouchCanceled;
        
        inputActions.Player.Interact.performed -= InteractPerformed;
        inputActions.Player.Interact.canceled -= InteractCanceled;

        inputActions.Player.Fire.performed -= FirePerformed;
        inputActions.Player.Fire.canceled -= FireCanceled;
        
        inputActions.Player.Aim.performed -= AimPerformed;
        inputActions.Player.Aim.canceled -= AimCanceled;

        inputActions.Player.Look.performed -= LookPerformed;
        inputActions.Player.Look.canceled -= LookCanceled;
    }

    void AwakingController()
    {
        if(Instance!=null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        inputActions = new InputActions();
    }

    private void UpdateActiveDevice(InputDevice device)
{
    ActiveDevice detected = device is Gamepad ? ActiveDevice.Gamepad : ActiveDevice.Mouse;
    
    if (detected != CurrentActiveDevice)
    {
        CurrentActiveDevice = detected;
        OnActiveDeviceChanged?.Invoke(CurrentActiveDevice);
    }
}

    #region Move
    void MovePerformed(InputAction.CallbackContext ctx)
    {
        OnMove?.Invoke(ctx.ReadValue<Vector2>());
    }
    void MoveCanceled(InputAction.CallbackContext ctx)
    {
        OnMove?.Invoke(Vector2.zero);
    }

    #endregion

    #region Sprint
    void SprintPerformed(InputAction.CallbackContext ctx)
    {
        OnSprint?.Invoke(true);
    }
    void SprintCanceled(InputAction.CallbackContext ctx)
    {
        OnSprint?.Invoke(false);
    }
    #endregion

    #region Crouch
    void CrouchPerformed(InputAction.CallbackContext ctx)
    {
        OnCrouch?.Invoke(true);
    }
    void CrouchCanceled(InputAction.CallbackContext ctx)
    {
        OnCrouch?.Invoke(false);
    }
    #endregion

    #region Interact

    void InteractPerformed(InputAction.CallbackContext ctx)
    {
        OnInteract?.Invoke(true);
    }
    void InteractCanceled(InputAction.CallbackContext ctx)
    {
        OnInteract?.Invoke(false);
    }

    #endregion

    #region Fire

    void FirePerformed(InputAction.CallbackContext ctx)
    {
        OnFire?.Invoke(true);
    }
    void FireCanceled(InputAction.CallbackContext ctx)
    {
        OnFire?.Invoke(false);
    }

    #endregion

    #region Aim

    void AimPerformed(InputAction.CallbackContext ctx)
    {
        OnAim?.Invoke(true);
    }
    void AimCanceled(InputAction.CallbackContext ctx)
    {
        OnAim?.Invoke(false);
    }

    #endregion

    #region Look

    void LookPerformed(InputAction.CallbackContext ctx)
    {
        UpdateActiveDevice(ctx.control.device);

         if (CurrentActiveDevice == ActiveDevice.Mouse && ctx.control.device is Mouse)
        {
           OnMouseLook?.Invoke(ctx.ReadValue<Vector2>()); 
        }
        else if (CurrentActiveDevice == ActiveDevice.Gamepad && ctx.control.device is Gamepad)
        {
           OnGamepadLook?.Invoke(ctx.ReadValue<Vector2>()); 
        }
        
        
    }
    void LookCanceled(InputAction.CallbackContext ctx)
    {
        if (CurrentActiveDevice == ActiveDevice.Mouse && ctx.control.device is Mouse)
    {
        OnMouseLook?.Invoke(Vector2.zero);
    }
    else if (CurrentActiveDevice == ActiveDevice.Gamepad && ctx.control.device is Gamepad)
    {
        OnGamepadLook?.Invoke(Vector2.zero);
    }
    }

    #endregion
}
