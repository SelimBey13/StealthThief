using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector2 mouseLookInput;
    private Vector2 gamepadLookInput;
    [SerializeField] private Transform playerOffSet;
    [SerializeField] private float cameraRadius;
    [SerializeField] private float cameraSphereRadius;
    [SerializeField] private bool isAiming;
    [SerializeField] float mouseSensitivityX;
    [SerializeField] float mouseSensitivityY;
    [SerializeField] float gamepadSensitivityX;
    [SerializeField] float gamepadSensitivityY;
    private Vector3 cameraConstantTransformVector;
    [SerializeField] bool canCameraTurn; // inspector takibi icin

    //private Vector3 cameraConstantDirection = new Vector3(0.615f, 0f, -1.1f);;
    private float yaw;
    private float pitch;
    [SerializeField] LayerMask solidArticle;

    void Start()
{
    pitch = 6f;
    yaw = -10f;
    cameraConstantTransformVector = new Vector3(0.41f,-0.12f,-1.18f);
}
    void Update()
    {
        SetCameraInputs(); 
    }

    void LateUpdate()
    {
        SetCameraTransform();
    }


    void OnEnable()
    {
        InputManager.OnMouseLook += MouseLookingInputs;
        InputManager.OnGamepadLook += GamepadLookingInputs;
    }
    
    void OnDisable() 
    {
        InputManager.OnMouseLook -= MouseLookingInputs;
        InputManager.OnGamepadLook -= GamepadLookingInputs;
    }
    
    void SetCameraInputs()
    {
        yaw += (mouseLookInput.x * mouseSensitivityX + gamepadLookInput.x * gamepadSensitivityX)*Time.deltaTime;
        pitch -= (mouseLookInput.y * mouseSensitivityY + gamepadLookInput.y * gamepadSensitivityY)*Time.deltaTime;
        pitch = Mathf.Clamp(pitch,-60f,75f);  
 
    }
    void SetCameraTransform()
    {
        Quaternion rotation = Quaternion.Euler(pitch,yaw,0);
        Vector3 targetPosition = playerOffSet.position + rotation*cameraConstantTransformVector.normalized * cameraRadius;

        Vector3 rayDirection = targetPosition - playerOffSet.position;
        float rayDistance = rayDirection.magnitude;                   

        if(Physics.SphereCast(playerOffSet.position,cameraSphereRadius,rayDirection.normalized,out RaycastHit hit,rayDistance,solidArticle))
        { 
            transform.position = hit.point + hit.normal *0.05f;
        }
        else
        {
            transform.position = targetPosition;
        }
        transform.rotation = rotation;

    }
    
    
    void MouseLookingInputs(Vector2 lookInput)
    {
        mouseLookInput = lookInput;
    }
    void GamepadLookingInputs(Vector2 lookInput)
    {
        gamepadLookInput = lookInput;
    }
}
