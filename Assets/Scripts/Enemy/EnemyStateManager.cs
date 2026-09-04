using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    EnemyStates enemyState = EnemyStates.Idle;

    void Start()
    {
        enemyState = EnemyStates.Idle;
    }
    public void SetEnemyState(EnemyStates newEnemyState)
    {
        if(newEnemyState == enemyState) { return;}

        enemyState = newEnemyState;
    }

    public EnemyStates CurrentEnemyState()
    {
        return enemyState;
    }
}
