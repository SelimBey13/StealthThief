using System;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    Vector3 fireDirection;
    public Vector3 FireDirection => fireDirection;
    Rigidbody firedRigidbody;
    public Rigidbody FiredRigidbody => firedRigidbody;
    [SerializeField] RectTransform crosshairRectTransform;
    Vector3 crosshairScreenPosition;
    Vector3 viewportPoint;
    Ray ray;
    [SerializeField] Camera mainCamera;
    [SerializeField] private float bulletRange;
    public event Action<Enemy> OnPlayerShootEnemy;

    void Start()
    {
        crosshairScreenPosition = crosshairRectTransform.position;
    }
    void OnEnable()
    {
        PlayerStateManager.OnFire += PlayerShoot;
    }
    void OnDisable()
    {
        PlayerStateManager.OnFire -= PlayerShoot;
    }

    void PlayerShoot(bool value)
    {
        if(value && PlayerStateManager.Instance.IsAiming)
        {
            viewportPoint = mainCamera.ScreenToViewportPoint(crosshairScreenPosition);
            ray = mainCamera.ViewportPointToRay(viewportPoint);

            if(Physics.Raycast(ray, out RaycastHit hit,bulletRange))
            {
                Enemy enemy = hit.collider.GetComponentInParent<Enemy>();
                fireDirection = (hit.transform.position - transform.position).normalized;
                firedRigidbody = hit.rigidbody;
                if(enemy != null)
                {
                    OnPlayerShootEnemy?.Invoke(enemy);
                }
            }
        }
    }

}
