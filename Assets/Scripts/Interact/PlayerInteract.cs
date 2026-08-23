using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    Collider[] colliderList;
    [SerializeField] float interactRadius;
    [SerializeField] LayerMask interactableLayer;
    Interactable nearestInteractable;
    float nearestDistance = Mathf.Infinity;
    Vector3 directionWith;
    Vector3 cameraDirectionForward;
    public static event Action<Interactable> OnNearestInteractable;
    private Coroutine coroutine;
    bool aktifMi = true;
    Interactable foundInteractable;

    void Start()
    {
        coroutine = StartCoroutine(Listeleyici());
        nearestInteractable = null;
    }

    void OnEnable()
    {
        InputManager.OnInteract += InteractInputs;
    }
    void OnDisable()
    {
        InputManager.OnInteract -= InteractInputs;
    }

    IEnumerator Listeleyici()
    {
        WaitForSeconds beklemeSuresi = new WaitForSeconds(0.10f);

        while(aktifMi)
        {
            ListTheInteractables();
            yield return beklemeSuresi;
        }

    }

    void ListTheInteractables()
{
    colliderList = Physics.OverlapSphere(transform.position, interactRadius, interactableLayer);
    cameraDirectionForward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);

    nearestDistance = Mathf.Infinity;
    foundInteractable = null;

    foreach(Collider collider in colliderList)
    {
        Interactable interactable = collider.GetComponent<Interactable>();
        if(interactable != null)
        {
            float temp = Vector3.Distance(transform.position, interactable.transform.position);
            directionWith = (interactable.transform.position - transform.position).normalized;
            float onForward = Vector3.Dot(cameraDirectionForward, directionWith);

            if((temp < nearestDistance) && onForward >= 0.2f)
            {
                nearestDistance = temp;
                foundInteractable = interactable;
            }
        }
    }

    if(foundInteractable != nearestInteractable)
    {
        nearestInteractable = foundInteractable;
        OnNearestInteractable?.Invoke(nearestInteractable);
    }
}
    void InteractInputs(bool value)
    {
        if(value && nearestInteractable != null)
        {
            nearestInteractable.Interact();
        }
    }
}
