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
        animator.SetFloat("IsCrouching",(PlayerStateManager.Instance.IsMouseCrouching || PlayerStateManager.Instance.CrouchLocked? 0f : 1f)
        ,0.15f,Time.deltaTime);
        animator.SetFloat("MoveX", MoveInput.x, 0.2f, Time.deltaTime);
        animator.SetFloat("MoveY", MoveInput.y, 0.2f, Time.deltaTime);
    }
}
