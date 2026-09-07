using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    EnemyStates enemyState = EnemyStates.Idle;
    void Start()
    {
        enemyState = EnemyStates.Idle;
    }
    void Update()
    {
        Debug.Log("EnemyState" + CurrentEnemyState());
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
