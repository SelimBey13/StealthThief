using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance{get; private set;}
   [SerializeField] private PlayerLiving playerLiving;
   public static event Action<bool> OnChallengeCompletedSuccesfully;

    [SerializeField] private bool canEscape = false;
    public bool CanEscape => canEscape;
    private bool isGameOver = false;
    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        ValuableInteractable.OnTakenItemNumber += SetItemNumber;
        EscapeCarInteractable.OnPlayerEscapedWithCar += GameOver;
        playerLiving.OnPlayerDead += GameOver;
    }
    void OnDisable()
    {
        ValuableInteractable.OnTakenItemNumber -= SetItemNumber;
        EscapeCarInteractable.OnPlayerEscapedWithCar -= GameOver;
        playerLiving.OnPlayerDead -= GameOver;
    }

    void SetItemNumber()
    {
        canEscape = true;
    }

    void GameOver()
    {
        Debug.Log("oyunbittila");
        if(isGameOver){return;}
        isGameOver = true;
        if(playerLiving.IsPlayerAlive)
        {
            OnChallengeCompletedSuccesfully?.Invoke(true);
        }
        else
        {
            OnChallengeCompletedSuccesfully?.Invoke(false);
        }
        
    }

}
