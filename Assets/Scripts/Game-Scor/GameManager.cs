using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance{get; private set;}
   public static event Action<bool> OnChallengeCompleted;

    [SerializeField] private bool canEscape = false;
    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        ValuableInteractable.OnTakenItemNumber += SetItemNumber;
    }
    void OnDisable()
    {
        ValuableInteractable.OnTakenItemNumber -= SetItemNumber;
    }

    void SetItemNumber()
    {
        canEscape = true;
    }
    
    /*if(oyun bitme kosulları)
    {
        OnChallengeCompleted?.Invoke(true);
    }*/

}
