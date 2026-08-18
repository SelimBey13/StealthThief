using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class InteractableData : ScriptableObject
{
    [SerializeField] InteractableType interactableType;
    [SerializeField] string interactableName;
    [SerializeField] Image interactionIcon;
}
