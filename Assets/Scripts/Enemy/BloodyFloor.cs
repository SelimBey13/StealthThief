using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BloodyFloor : MonoBehaviour
{
    Enemy enemy;
    bool shotOnce;
    [SerializeField] private float bloodStartingTime;
    [SerializeField] private float bloodSpreadTime;
    DecalProjector projector;
    [SerializeField] private GameObject bloodDecalPrefab;
    Rigidbody shotRigidBody;
    [SerializeField] float bloodRayDistance;
    [SerializeField] private LayerMask layer;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    void Start()
    {
        shotOnce = false;
    }

    void OnEnable()
    {
        enemy.OnBloodFloor += BloodTheFloor;
    }
    void OnDisable()
    {
        enemy.OnBloodFloor -= BloodTheFloor;
    }

    void BloodTheFloor(Rigidbody rb)
    {
        shotRigidBody = rb;
        if(shotOnce){return;}
        StartCoroutine(BloodCoroutine(bloodSpreadTime));
        shotOnce = true;
    }

    IEnumerator BloodCoroutine(float spreadTime)
    {
        yield return new WaitForSeconds(bloodStartingTime);
        float time = 0f;

        Physics.Raycast(shotRigidBody.transform.position,Vector3.down,out RaycastHit hit,bloodRayDistance,layer);

        if(hit.collider != null)
        {
            GameObject liveBlood = Instantiate(bloodDecalPrefab,hit.point + hit.normal * 0.05f,Quaternion.LookRotation(-hit.normal));
            liveBlood.transform.SetParent(transform,true);
            projector = liveBlood.GetComponent<DecalProjector>();
            projector.fadeFactor = 0f;

            while(time<spreadTime)
            {
                time += Time.deltaTime;
                projector.fadeFactor = time/spreadTime;
                yield return null;
            }  
            projector.fadeFactor = 1f;
        }
        else
        {
            yield break;
        }
        
    }
}
