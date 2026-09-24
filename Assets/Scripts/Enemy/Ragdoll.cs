using System;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] ragdollBodyParts;
    [SerializeField] private float bulletForce;
    Enemy enemy;

    public event Action OnAnimationClosed;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        ragdollBodyParts = GetComponentsInChildren<Rigidbody>();
    }
    void OnEnable()
    {
        enemy.OnRagdollPhysic += ActivateRagdoll;
    }
    void OnDisable()
    {
        enemy.OnRagdollPhysic -= ActivateRagdoll;
    }

    void ActivateRagdoll(Vector3 direction, Rigidbody rb)
    {
        OnAnimationClosed?.Invoke();
        foreach(Rigidbody rigidbody in ragdollBodyParts)
        {
            rigidbody.isKinematic = false;
            rigidbody.gameObject.layer = LayerMask.NameToLayer("DeadBody");
        }
        rb.AddForce(direction * bulletForce,ForceMode.Impulse);
        
    }
}
