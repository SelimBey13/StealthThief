using UnityEngine;

public class ValuableInteractable : Interactable
{
    [SerializeField] int valuableLevel; // 1-Cheap 2-Mid 3-Expensive
    [SerializeField] float value; // 1*değer =2, 2*değer =3;

    public override void Interact()
    {
        gameObject.SetActive(false);
        Debug.Log("item alindi test");
    }


}
