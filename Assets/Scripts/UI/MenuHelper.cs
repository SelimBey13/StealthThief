using UnityEngine;
using UnityEngine.EventSystems;

public class MenuHelper : MonoBehaviour
{
    public static MenuHelper Instance{get; private set;}
    [SerializeField] private GameObject firstButton;
    private ActiveDevice activeDevice;

    void Awake()
    {
        AwakingController();
    }

    void OnEnable()
    {
        MainMenuController.OnFirstButtonChanged += FirstButtonInformation;
        PauseMenuController.OnFirstButtonChanged += FirstButtonInformation;
        InputManager.OnActiveDeviceChanged += SetActiveDevice;

    }
    void OnDisable()
    {
        MainMenuController.OnFirstButtonChanged -= FirstButtonInformation;
        PauseMenuController.OnFirstButtonChanged -= FirstButtonInformation;
        InputManager.OnActiveDeviceChanged -= SetActiveDevice;
    }

    void AwakingController()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }   
        Instance = this; 
        DontDestroyOnLoad(gameObject);
    }

    void FirstButtonInformation(GameObject firstButton)
    {
        this.firstButton = firstButton;
        SetFirstButton();
        SetCursorForMenu();
    }
    void SetActiveDevice(ActiveDevice activeDevice)
    {
        this.activeDevice = activeDevice;
        SetFirstButton();

        if(InputManager.Instance.IsMenuMapActive)
        {
        SetCursorForMenu();
        }
    }

    public void SetCursorForGameplay() // gameplaye geçmeyi sağlayan unity eventlere
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;    
    }
    public void SetCursorForMenu() // menüye geçmeyi sağlayan unity eventlere
    {
        if(activeDevice == ActiveDevice.Mouse)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
        Cursor.lockState = CursorLockMode.None;
    }

    public void SetFirstButton()
    {
        if(activeDevice == ActiveDevice.Gamepad)
        {
            EventSystem.current.SetSelectedGameObject(firstButton); 
            
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
