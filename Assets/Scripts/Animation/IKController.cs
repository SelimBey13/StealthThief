using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;

public class IKController : MonoBehaviour
{
    Animator anim;
    [SerializeField] RectTransform crosshairRectTransform;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void OnAnimatorIK(int layerIndex)
    {
        if(layerIndex != 1) return;

        if(PlayerStateManager.Instance.IsAiming)
        {
            Vector3 crosshairScreenPos = crosshairRectTransform.position;
            Vector3 viewportPoint = Camera.main.ScreenToViewportPoint(crosshairScreenPos);
            Ray ray = Camera.main.ViewportPointToRay(viewportPoint);

            Vector3 targetPosition;
            

            if(Physics.Raycast(ray,out RaycastHit hit, 100f))
            {
                targetPosition = hit.point;
            }
            else
            {
                targetPosition = ray.GetPoint(50f);
            }

            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            anim.SetIKPosition(AvatarIKGoal.RightHand, targetPosition);

            anim.SetLookAtWeight(0.5f); // ne kadar etkili olsun, test ede ede ayarlarsın (0.3-0.7 arası makul)
            anim.SetLookAtPosition(targetPosition);
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand,0f);
        }
    }
}
