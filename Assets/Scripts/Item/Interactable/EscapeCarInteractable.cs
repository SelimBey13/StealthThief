using System;

public class EscapeCarInteractable : Interactable
{
    public static event Action OnPlayerEscapedWithCar;
    public static event Action OnPlayerCantEscapeWithoutMoney;

    public override void Interact()
    {
        if(GameManager.Instance.CanEscape)
        {
            OnPlayerEscapedWithCar?.Invoke();
        }
        else
        {
            OnPlayerCantEscapeWithoutMoney?.Invoke();
        }
        
    }
}
