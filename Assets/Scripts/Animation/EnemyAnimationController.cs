using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    EnemyStateManager enemyStateManager;
    Animator animator;
    Ragdoll ragdoll;

    void Awake()
    {
        ragdoll = GetComponent<Ragdoll>();
        enemyStateManager = GetComponent<EnemyStateManager>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        ragdoll.OnAnimationClosed += CloseAnimator;
    }
    void OnDisable()
    {
        ragdoll.OnAnimationClosed -= CloseAnimator;
    }
    void Update()
    {
        SetAnimations();
    }

    void SetAnimations()
    {
        animator.SetInteger("StateIndex", (int)enemyStateManager.CurrentEnemyState());
    }

    void CloseAnimator()
    {
        animator.enabled = false;
        enabled = false;
    }

}
