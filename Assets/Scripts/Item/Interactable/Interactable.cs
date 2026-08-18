using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    [SerializeField] InteractableData interactableData;
    public abstract void Interact();
}
