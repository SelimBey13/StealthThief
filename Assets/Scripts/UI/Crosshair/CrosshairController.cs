using UnityEngine.UI;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    Image crosshair;

    void Awake()
    {
        crosshair = GetComponent<Image>();
    }
    void Update()
    {
        ControlAiming();
    }
    void ControlAiming()
    {
        crosshair.enabled = PlayerStateManager.Instance.IsAiming;
    }
}
