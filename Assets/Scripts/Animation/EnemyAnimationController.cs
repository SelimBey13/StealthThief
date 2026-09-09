using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    EnemyStateManager enemyStateManager;
    Animator animator;

    void Awake()
    {
        enemyStateManager = GetComponent<EnemyStateManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        SetAnimations();
    }

    void SetAnimations()
    {
        animator.SetInteger("StateIndex", (int)enemyStateManager.CurrentEnemyState());
    }

}
