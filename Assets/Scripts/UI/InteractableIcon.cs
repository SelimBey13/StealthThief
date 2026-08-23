using UnityEngine;
using UnityEngine.UI;

public class InteractableIcon : MonoBehaviour
{
    [SerializeField] private GameObject interactableIconCanvas;
    [SerializeField] private Image imageBox;
    [SerializeField] private float distanceFromPosition;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Camera mainCamera;
    Vector3 screenPos;

    Interactable selectedInteractable;
    ActiveDevice activeDevice;
    Sprite icon;

    void Awake()
    {
        interactableIconCanvas.SetActive(false);  
    }
    void Update()
    {
        if(selectedInteractable)
        {
            screenPos = mainCamera.WorldToScreenPoint(selectedInteractable.IconTransform.position);
            interactableIconCanvas.transform.position = screenPos;
        }
    }
    void OnEnable()
    {
        PlayerInteract.OnNearestInteractable += SelectInteractable;
        InputManager.OnActiveDeviceChanged += SetActiveDevice;
    }
    void OnDisable()
    {
        PlayerInteract.OnNearestInteractable -= SelectInteractable;
        InputManager.OnActiveDeviceChanged -= SetActiveDevice;
    }
    
    void SetIcon()
    {
        if(activeDevice == ActiveDevice.Mouse)
        {
            icon = selectedInteractable.InteractableData.KeyboardIcon;
        }
        else if(activeDevice == ActiveDevice.Gamepad)
        {
            icon = selectedInteractable.InteractableData.GamepadIcon;
        }
    }

    void PutIconInsideImage()
    {
        if(icon == null) return;
        imageBox.sprite = icon;
    }
    void SelectInteractable(Interactable interactable)
    {
        selectedInteractable = interactable;
        
        if(interactable)
        {
            SetIcon();
            interactableIconCanvas.SetActive(true);   
            PutIconInsideImage();    
        }
        else
        {
            interactableIconCanvas.SetActive(false);  
        }
    }

    void SetActiveDevice(ActiveDevice activeDevice)
    {
        this.activeDevice = activeDevice;

        if(selectedInteractable)
        {
            SetIcon();
            PutIconInsideImage();
        }
    }

}
