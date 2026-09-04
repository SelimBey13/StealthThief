using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyStateManager enemyStateManager;
    [SerializeField] private float moveTime;

    void Awake()
    {
        enemyStateManager = GetComponent<EnemyStateManager>();
    }
    void Start()
    {
        StartCoroutine(WaitForStart(moveTime));
    }

    IEnumerator WaitForStart(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        enemyStateManager.SetEnemyState(EnemyStates.Patrol);
    }

}
