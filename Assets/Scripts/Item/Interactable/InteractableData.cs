using UnityEngine;

[CreateAssetMenu]
public class InteractableData : ScriptableObject
{
    [SerializeField] InteractableType interactableType;
    [SerializeField] string interactableName;
    [SerializeField] private Sprite keyboardIcon;
    [SerializeField] private Sprite gamepadIcon;

    public Sprite KeyboardIcon => keyboardIcon;
    public Sprite GamepadIcon => gamepadIcon;
}
