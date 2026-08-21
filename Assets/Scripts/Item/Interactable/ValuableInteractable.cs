using System;
using UnityEngine;

public class ValuableInteractable : Interactable
{
    [SerializeField] int valuableLevel;
    [SerializeField] float value;
    public static event Action<float> OnItemTaken;
    public static event Action OnTakenItemNumber;

    public override void Interact()
    {
        gameObject.SetActive(false);
        Debug.Log("item alindi test");
        OnItemTaken?.Invoke(value);
        OnTakenItemNumber?.Invoke();
        
    }


}
