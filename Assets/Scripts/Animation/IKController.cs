using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;

public class IKController : MonoBehaviour
{
    Animator anim;
    [SerializeField] RectTransform crosshairRectTransform;
    Vector3 smoothedTarget;

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

            smoothedTarget = Vector3.Lerp(smoothedTarget, targetPosition, 15f * Time.deltaTime);

            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            anim.SetIKPosition(AvatarIKGoal.RightHand, smoothedTarget);

            anim.SetLookAtWeight(
                weight: 1f,      // genel etki
                bodyWeight: 0.3f, // GÖVDENİN ne kadar döneceği
                headWeight: 0.6f, // KAFANIN ne kadar döneceği
                eyesWeight: 0.5f,  // gözlerin ne kadar döneceği
                clampWeight: 0.5f  // aşırı bükülmeyi sınırlama
                );
            anim.SetLookAtPosition(smoothedTarget);
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand,0f);
        }
    }
}
