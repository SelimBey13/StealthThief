using System;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    Vector3 fireDirection;
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
                Debug.Log($"Raycast hit: {hit.collider?.name}");
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if(enemy != null)
                {
                    OnPlayerShootEnemy?.Invoke(enemy);
                }
            }
        }
    }

}
