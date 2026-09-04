using System.Collections;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
public class EnemyVision : MonoBehaviour
{
    private float distance;
    Enemy enemy;
    [SerializeField] Transform playerTransform;
    [SerializeField] private float fairDistance;
    [SerializeField] private float sphereRadius;
    Collider[] colliderList;
    float dotProduct;
    bool isPlayerInside;
    int seenBodyParts;
    float seeingRate = 0.7f;
    [SerializeField] Transform[] playerBodyParts;
    [SerializeField] private float enemyHeight;
    [SerializeField] Transform enemyEyeTransform;
    [SerializeField] private LayerMask obstacleLayer;
    bool isCurrentlySeeing = false;
    Coroutine seeingCoroutine;


    void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    void Start()
    {
        StartCoroutine(DistanceWithPlayer());
    }

    IEnumerator DistanceWithPlayer()
    {
        float distance;
        WaitForSeconds beklemeSuresi = new WaitForSeconds(0.1f);
        bool loop = true;
        while(loop)
        {
            distance = Vector3.Distance(playerTransform.position , transform.position);
            yield return beklemeSuresi;
            if(distance<fairDistance)
            {
                ControlAround();
            }
        }
    }
    void ControlAround()
    {
        WhereIsPlayer();
        IsPlayerInSphere();
        CanSeeAllParts();

        if(isPlayerInside && (seenBodyParts>= playerBodyParts.Length*seeingRate))
        {
            if(isCurrentlySeeing == false)
            {
                isCurrentlySeeing = true;
                seeingCoroutine = StartCoroutine(GiveChanceToPlayer());
            }
        }
        else
        {
            isCurrentlySeeing = false;
            StopCoroutine(seeingCoroutine);
        }

    }

    void WhereIsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        dotProduct =  Vector3.Dot(transform.forward.normalized,direction);
    }

    void IsPlayerInSphere()
    {
        colliderList = Physics.OverlapSphere(transform.position,sphereRadius);
        isPlayerInside = false;
        foreach(Collider colliders in colliderList)
        {
            if(colliders != null)
            {
                if(colliders.GetComponent<PlayerMovement>())
                {
                    isPlayerInside = true;
                    return;
                }
            }
        }
    }

    void CanSeeAllParts()
    {
        if(dotProduct <0)
        {
            seenBodyParts = 0;
            return;
        }

        int value = 0;
        Vector3 direction;
        float distance;
        foreach(Transform bodyPart in playerBodyParts)
        {
            direction = (bodyPart.position - enemyEyeTransform.position).normalized;
            distance = Vector3.Distance(bodyPart.position,enemyEyeTransform.position);
            if(!Physics.Raycast(enemyEyeTransform.position,direction,distance,obstacleLayer))
            {
                value ++;
            }
        }

        seenBodyParts = value;
    
    }

    IEnumerator GiveChanceToPlayer()
    {
        float time;
        if(seenBodyParts ==0)
        {
            time = 0.5f;
        }
        else
        {
            time = 0.5f + ((float)playerBodyParts.Length/seenBodyParts) * 0.1f;
        }
        yield return new WaitForSeconds(time);
        if(isCurrentlySeeing)
        {
            //OnBLABLA.INVOKE
        }
    }

}

