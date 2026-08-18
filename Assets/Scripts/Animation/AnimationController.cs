using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;
    Vector2 MoveInput;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        SetMainAnimations();
        SetSubAnimations();
        SetAimLayerAnimations();
    }

    void OnEnable()
    {
        InputManager.OnMove += MoveInputs;
    }
    void OnDisable()
    {
        InputManager.OnMove -= MoveInputs;
    }

    void MoveInputs(Vector2 value)
    {
        MoveInput = value;
    }

    void SetMainAnimations()
    {
        animator.SetInteger("MainState", (int)PlayerStateManager.Instance.CurrentMainState);
    }
    void SetSubAnimations()
    {
        bool isCrouching = PlayerStateManager.Instance.IsMouseCrouching || PlayerStateManager.Instance.CrouchLocked;
        animator.SetFloat("IsCrouching",isCrouching ? 0f : 1f
        ,0.15f,Time.deltaTime);
        animator.SetFloat("MoveX", MoveInput.x, 0.6f, Time.deltaTime);
        animator.SetFloat("MoveY", MoveInput.y, 0.6f, Time.deltaTime);
    }

    void SetAimLayerAnimations()
    {
            animator.SetBool("IsAiming",PlayerStateManager.Instance.IsAiming);
            animator.SetLayerWeight(1,PlayerStateManager.Instance.IsAiming? 1f : 0f);
    }

}
