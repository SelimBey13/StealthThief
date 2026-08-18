using System.Collections;
using UnityEngine;

public class LeftDoorInteractable : Interactable
{
    [SerializeField] float turnValue;
    [SerializeField] Transform hingeTransform;
    [SerializeField] bool isOpen;
    [SerializeField] bool onWork;
    [SerializeField] Transform openedTarget;
    [SerializeField] Transform closedTarget;
    Coroutine turnDoor;

    void Awake()
    {
        isOpen = false;
        onWork = false;
    }

    public override void Interact()
    {   
        if(!onWork){turnDoor = StartCoroutine(KapiyiAc());}
        
    }

    

    IEnumerator KapiyiAc()
{
    onWork = true;
    float elapsedTime = 0f;
    Quaternion startRotation = hingeTransform.rotation;
    Quaternion targetRotation = isOpen ? closedTarget.rotation : openedTarget.rotation;

    while (elapsedTime < turnValue)
    {
        float t = elapsedTime / turnValue;
        hingeTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

        elapsedTime += Time.deltaTime;
        yield return null;
    }

    hingeTransform.rotation = targetRotation;
    isOpen = !isOpen;
    onWork = false;
}
}
