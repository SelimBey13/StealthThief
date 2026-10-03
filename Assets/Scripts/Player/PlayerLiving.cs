using System;
using UnityEngine;

public class PlayerLiving : MonoBehaviour
{
    private bool isPlayerAlive;
    public bool IsPlayerAlive => isPlayerAlive;

    public event Action OnPlayerDead;

    void Start()
    {
        isPlayerAlive = true;
    }
    void OnEnable()
    {
        Enemy.OnPlayerShot += PlayerLifeSituation;
    }
    void OnDisable()
    {
        Enemy.OnPlayerShot -= PlayerLifeSituation;
    }

    void PlayerLifeSituation()
    {
        if(isPlayerAlive)
        {
            isPlayerAlive = false;
            OnPlayerDead?.Invoke();  
        }
        
    }
}
