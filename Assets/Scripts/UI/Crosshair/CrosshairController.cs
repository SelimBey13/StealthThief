using UnityEngine.UI;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    Image crosshair;

    void Awake()
    {
        crosshair = GetComponent<Image>();
        Cursor.visible = false;
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
