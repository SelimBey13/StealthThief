using System;
using System.Collections;
using UnityEngine;
public class EnemyVision : MonoBehaviour
{
    Enemy enemy;
    [SerializeField] Transform playerTransform;
    [SerializeField] private float fairDistance;
    [SerializeField] private float sphereRadius;
    public float SphereRadius=>sphereRadius;
    [SerializeField] Transform[] playerBodyParts;
    float playerBodyPartNumber;
    [SerializeField] private float enemyHeight;
    [SerializeField] Transform enemyEyeTransform;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float escapeTime;
    Vector3 lastPlayerLocation;
    Collider[] colliderList;
    float dotProduct;
    public float DotProduct => dotProduct;
    float distance;
    bool isPlayerEscaped;
    bool isPlayerInside;
    int seenBodyParts;
    float seeingRate;
    float baseSeeingRate;
    bool isCurrentlySeeing = false;
    Coroutine seeingCoroutine;
    Coroutine escapingCoroutine;
    EnemyVoice enemyVoice;
    public event Action OnEnemyAlert;
    public event Action OnEnemyShootPlayer;
    public event Action<Vector3> OnEnemyEscaped;
    private float voiceLevel;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyVoice = GetComponent<EnemyVoice>();
    }
    void Start()
    {
        isPlayerEscaped = false;
        playerBodyPartNumber = playerBodyParts.Length;
        baseSeeingRate = 0.7f; 
    }

    void OnEnable()
    {
        enemy.OnDistanceChanged += DistanceWithPlayer;
        enemyVoice.OnVoiceLevelChanged += ReceiveVoiceLevel;
    }
    void OnDisable()
    {
        enemy.OnDistanceChanged -= DistanceWithPlayer;
        enemyVoice.OnVoiceLevelChanged -= ReceiveVoiceLevel;
    }
    void DistanceWithPlayer(float value)
    {
        distance = value;
        if(distance<fairDistance)
        {
            ControlAround();
        }
    }
    void ControlAround()
    {
        WhereIsPlayer();
        IsPlayerInSphere();
        CanSeeAllParts();
        SetSeeingRate();

    //Debug.Log($"isPlayerInside: {isPlayerInside}, seenBodyParts: {seenBodyParts}, gereken: {playerBodyPartNumber*seeingRate}, dotProduct: {dotProduct}");

        if(isPlayerInside && (seenBodyParts>= playerBodyPartNumber*seeingRate))
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
            if(seeingCoroutine !=null)
            {
               StopCoroutine(seeingCoroutine); 
            }
            
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
            OnEnemyAlert?.Invoke(); 
        }
    }

    public void SpecifyLastLocation()
    {
        escapingCoroutine = StartCoroutine(CheckIfEscaped());
    }

    IEnumerator CheckIfEscaped()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);
        float time = 0f;
        lastPlayerLocation = playerTransform.position; // ne olur ne olmaz ilk değer ataması
        while(time < escapeTime)
        {
            yield return waitTime;
            time += 0.1f;
            if(isPlayerInside)
            {
                isPlayerEscaped = false;
                lastPlayerLocation = playerTransform.position;
            }
            else
            {
                isPlayerEscaped = true;
                OnEnemyEscaped?.Invoke(lastPlayerLocation);
                yield break;
                
                
            }
        }

        OnEnemyShootPlayer?.Invoke();

    }

    void ReceiveVoiceLevel(float value)
    {
        voiceLevel = value;
    }

    void SetSeeingRate()
    {
        seeingRate = baseSeeingRate - 0.7f*voiceLevel/100f;
    }
}

