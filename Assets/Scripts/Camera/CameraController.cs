using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector2 mouseLookInput;
    private Vector2 gamepadLookInput;
    [SerializeField] Transform playerVisualTransform;
    [SerializeField] private Transform playerOffSet;
    [SerializeField] private float cameraRadius;
    [SerializeField] private float aimCameraRadius;
    [SerializeField] private float cameraSphereRadius;
    [SerializeField] private float radiusSpeed;
    private float smoothedRadius;
    Vector3 currentOffset; 
    [SerializeField] private bool isAiming;
    [SerializeField] float mouseSensitivityX;
    [SerializeField] float mouseSensitivityY;
    [SerializeField] float gamepadSensitivityX;
    [SerializeField] float gamepadSensitivityY;
    private Vector3 cameraConstantTransformVector = new Vector3(0.608f, 0.015f, -1.572f);
    private Vector3 aimCameraTransformVector = new Vector3(0.469f, 0.036f, -0.523f);
    private float yaw;
    private float pitch;
    [SerializeField] LayerMask solidArticle;

    void Start()
{
    pitch = 1.933f;
    yaw = -1.543f;
    smoothedRadius = cameraRadius;
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
        if(PlayerStateManager.Instance.IsAiming)
        {
            yaw = playerVisualTransform.eulerAngles.y;
            pitch -= (mouseLookInput.y * mouseSensitivityY + gamepadLookInput.y * gamepadSensitivityY)*Time.deltaTime;
            pitch = Mathf.Clamp(pitch,-60f,75f);  
            return;
        }
        yaw += (mouseLookInput.x * mouseSensitivityX + gamepadLookInput.x * gamepadSensitivityX)*Time.deltaTime;
        pitch -= (mouseLookInput.y * mouseSensitivityY + gamepadLookInput.y * gamepadSensitivityY)*Time.deltaTime;
        pitch = Mathf.Clamp(pitch,-60f,75f);  
 
    }
    void SetCameraTransform()
    {
        Vector3 targetOffset;

        bool isCrouching = PlayerStateManager.Instance.IsMouseCrouching || PlayerStateManager.Instance.CrouchLocked;
        bool isAiming = PlayerStateManager.Instance.IsAiming;
        if (isAiming && isCrouching)
        {
            targetOffset = aimCameraTransformVector - new Vector3(0f, 0.38f, 0f);
        }
        else if(isAiming)
        {
            targetOffset = aimCameraTransformVector;
        }
        else if (isCrouching)
        {
            targetOffset = cameraConstantTransformVector - new Vector3(0f, 0.38f, 0f);
        }
        else
        {
            targetOffset = cameraConstantTransformVector;
        }
        float targetRadius = PlayerStateManager.Instance.IsAiming ? aimCameraRadius : cameraRadius;
        smoothedRadius = Mathf.Lerp(smoothedRadius,targetRadius,radiusSpeed * Time.deltaTime);

        currentOffset = Vector3.Lerp(currentOffset, targetOffset, 5f * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(pitch,yaw,0);
        Vector3 targetPosition = playerOffSet.position + rotation * currentOffset.normalized * smoothedRadius;
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
