using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEditor;
public class InputManager : MonoBehaviour
{
    public static InputManager Instance {get; private set;}
    private InputActions inputActions;
    public static event Action<Vector2> OnMove;
    public static event Action<bool> OnMouseSprint; //Mouse = Klavye-Mouse Sistemi
    public static event Action<bool> OnGamepadSprint;
    public static event Action<bool> OnMouseCrouch;
    public static event Action<bool> OnGamepadCrouch;
    public static event Action<bool> OnInteract;
    public static event Action<bool> OnFire;
    public static event Action<bool> OnAim;
    public static event Action<bool> OnPause;
    public static event Action<Vector2> OnMouseLook;
    public static event Action<Vector2> OnGamepadLook;
    public static event Action<string> OnMouseInventory;
    public static event Action<string> OnGamepadInventory;
    public static ActiveDevice CurrentActiveDevice { get; private set; } = ActiveDevice.Mouse;
    public static event Action<ActiveDevice> OnActiveDeviceChanged;// UI Degisim kısmında kullanılacak
    public bool IsMenuMapActive{get; private set;}

    void Awake()
    {
        AwakingController();
        IsMenuMapActive = true;
    }

   void Start()
{
    SwitchToMenuMap();
}

    void Update()
    {
        Debug.Log("Player map aktif mi: " + inputActions.Player.enabled);
        Debug.Log("Menu map aktif mi: " + inputActions.Menu.enabled);
    }
    void OnEnable()
    {
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

        inputActions.Player.Pause.performed += PausePerformed;
        inputActions.Player.Pause.canceled += PauseCanceled;

        inputActions.Player.WeaponInventory.performed += InventoryPerformed;

        inputActions.Menu.Pause.performed += PausePerformed;
        inputActions.Menu.Pause.canceled += PauseCanceled;

        // MENU

        inputActions.Menu.Point.performed += ActiveDeviceForMenu;
        inputActions.Menu.Point.canceled += ActiveDeviceForMenu;

        inputActions.Menu.LeftClick.performed += ActiveDeviceForMenu;
        inputActions.Menu.LeftClick.canceled += ActiveDeviceForMenu;

        inputActions.Menu.Submit.performed += ActiveDeviceForMenu;
        inputActions.Menu.Submit.canceled += ActiveDeviceForMenu;

        inputActions.Menu.Cancel.performed += ActiveDeviceForMenu;
        inputActions.Menu.Cancel.canceled += ActiveDeviceForMenu;

        inputActions.Menu.Move.performed += ActiveDeviceForMenu;
        inputActions.Menu.Move.canceled += ActiveDeviceForMenu;
        
    }

    void OnDisable()
    {
        if(inputActions == null) return;

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
        
        inputActions.Player.Pause.performed -= PausePerformed;
        inputActions.Player.Pause.canceled -= PauseCanceled;

        inputActions.Player.WeaponInventory.performed -= InventoryPerformed;

        inputActions.Menu.Pause.performed -= PausePerformed;
        inputActions.Menu.Pause.canceled -= PauseCanceled;

        //MENU

        inputActions.Menu.Point.performed -= ActiveDeviceForMenu;
        inputActions.Menu.Point.canceled -= ActiveDeviceForMenu;

        inputActions.Menu.LeftClick.performed -= ActiveDeviceForMenu;
        inputActions.Menu.LeftClick.canceled -= ActiveDeviceForMenu;

        inputActions.Menu.Submit.performed -= ActiveDeviceForMenu;
        inputActions.Menu.Submit.canceled -= ActiveDeviceForMenu;

        inputActions.Menu.Cancel.performed -= ActiveDeviceForMenu;
        inputActions.Menu.Cancel.canceled -= ActiveDeviceForMenu;

        inputActions.Menu.Move.performed -= ActiveDeviceForMenu;
        inputActions.Menu.Move.canceled -= ActiveDeviceForMenu;
    }

    public void SwitchToMenuMap()
    {    
        inputActions.Player.Disable();
        inputActions.Menu.Enable();
        IsMenuMapActive = true;
        MenuHelper.Instance.SetCursorForMenu();
    }
    public void SwitchToPlayerMap()
    {
        inputActions.Menu.Disable();
        inputActions.Player.Enable();  
        IsMenuMapActive = false;
        MenuHelper.Instance.SetCursorForGameplay();
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
        OnActiveDeviceChanged?.Invoke(CurrentActiveDevice); // UI Degisim kısmında kullanılacak
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
        UpdateActiveDevice(ctx.control.device);
        if(CurrentActiveDevice == ActiveDevice.Mouse && ctx.control.device is Keyboard)
        {
            OnMouseSprint?.Invoke(true);
        }
        else if(CurrentActiveDevice == ActiveDevice.Gamepad && ctx.control.device is Gamepad)
        {
            OnGamepadSprint?.Invoke(true);
        }
        
    }
    void SprintCanceled(InputAction.CallbackContext ctx)
    {
        if(CurrentActiveDevice == ActiveDevice.Mouse && ctx.control.device is Keyboard)
        {
            OnMouseSprint?.Invoke(false);
        }
        else if(CurrentActiveDevice == ActiveDevice.Gamepad && ctx.control.device is Gamepad)
        {
            OnGamepadSprint?.Invoke(false);
        }
    }
    #endregion

    #region Crouch
    void CrouchPerformed(InputAction.CallbackContext ctx)
    {
         UpdateActiveDevice(ctx.control.device);
         
        if(CurrentActiveDevice == ActiveDevice.Mouse && ctx.control.device is Keyboard)
        {
            OnMouseCrouch?.Invoke(true);
        }
        else if(CurrentActiveDevice == ActiveDevice.Gamepad && ctx.control.device is Gamepad)
        {
            OnGamepadCrouch?.Invoke(true);
        }
    }
    void CrouchCanceled(InputAction.CallbackContext ctx)
    {
        if(CurrentActiveDevice == ActiveDevice.Mouse && ctx.control.device is Keyboard)
        {
            OnMouseCrouch?.Invoke(false);
        }
        else if(CurrentActiveDevice == ActiveDevice.Gamepad && ctx.control.device is Gamepad)
        {
            OnGamepadCrouch?.Invoke(false);
        }
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

    #region Pause

    void PausePerformed(InputAction.CallbackContext ctx)
    {
        OnPause?.Invoke(true);
    }

    void PauseCanceled(InputAction.CallbackContext ctx)
    {
        OnPause?.Invoke(false);
    }
    #endregion

    #region ActiveDeviceMenu
    void ActiveDeviceForMenu(InputAction.CallbackContext ctx)
    {
        if(ctx.control == null) return;
        UpdateActiveDevice(ctx.control.device);
    }
    #endregion
    
    #region Inventory

    void InventoryPerformed(InputAction.CallbackContext ctx)
    {
        UpdateActiveDevice(ctx.control.device);
        string keyName = ctx.control.name;

        if(CurrentActiveDevice == ActiveDevice.Mouse && ctx.control.device is Keyboard)
        {
            OnMouseInventory?.Invoke(keyName);
        }
        else if(CurrentActiveDevice == ActiveDevice.Gamepad && ctx.control.device is Gamepad)
        {
            OnGamepadInventory?.Invoke(keyName);
        }
    }

    #endregion

    public void DisableWeaponInventory()
    {
        inputActions.Player.WeaponInventory.Disable();
    }
    public void EnableWeaponInventory()
    {
        inputActions.Player.WeaponInventory.Enable();
    }
}
