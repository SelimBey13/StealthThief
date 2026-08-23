using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    [SerializeField] private InteractableData interactableData;
    [SerializeField] private Transform iconTransform;
    public InteractableData InteractableData => interactableData;
    public Transform IconTransform => iconTransform;
    public abstract void Interact();
}
