using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform[] patrolTransform;
    Transform playerTransform;
    EnemyStateManager enemyStateManager;
    Enemy enemy;
    EnemyVision enemyVision;
    EnemyVoice enemyVoice;
    NavMeshAgent navMeshAgent;
    [SerializeField] float enemySpeed;
    [SerializeField] float enemySearchSpeed;
    int currentPatrol;
    int direction;
    bool isPatrol;
    bool isArrived;
    bool isSearching;
    bool isAlert;
    Coroutine loopCoroutine;
    Coroutine searchCoroutine;
    public event Action OnArrivedSearchPoint;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyStateManager = GetComponent<EnemyStateManager>();
        enemy = GetComponent<Enemy>();
        enemyVoice = GetComponent<EnemyVoice>();
        enemyVision = GetComponent<EnemyVision>();
    }

    void Start()
    {
        playerTransform = EnemyPlayerReferences.Instance.playerTransform;
        currentPatrol = 0;
        direction = 1;
    }

    void Update()
    {
        if(isAlert)
        {
            SetVisualToPlayer();
        }
    }

    void OnEnable()
    {
        enemy.OnFirstTimePatrol += GoStartingPatrolLocationFirstTime;
        enemy.OnEnemyStateChanged += IsPatrolControl;
        enemy.OnEnemyStateChanged += IsSearchingControl;
        enemy.OnEnemyStateChanged += IsAlertControl;
        enemy.OnAllCoroutinesMustStop += ResetEnemy;
    }

    void OnDisable()
    {
        enemy.OnFirstTimePatrol -= GoStartingPatrolLocationFirstTime;
        enemy.OnEnemyStateChanged -= IsPatrolControl;
        enemy.OnEnemyStateChanged -= IsSearchingControl;
        enemy.OnEnemyStateChanged -= IsAlertControl;
        enemy.OnAllCoroutinesMustStop -= ResetEnemy;
    }

    void IsArrived()
    {
        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            isArrived = true;
            SetCurrentPatrol();
        }
        else
        {
            isArrived = false;
        }
        
    }
    void SetCurrentPatrol()
    {
        currentPatrol += direction;
        if(currentPatrol == patrolTransform.Length-1 || currentPatrol == 0)
        {
          direction = -1*direction;
        }
        navMeshAgent.SetDestination(patrolTransform[currentPatrol].position);
     }

    IEnumerator PatrolLoop()
    {
        WaitForSeconds varisKontrol = new WaitForSeconds(0.2f);
        navMeshAgent.speed = enemySpeed;
        while(isPatrol)
        {
            yield return varisKontrol;
            IsArrived();
        }
    }

    IEnumerator SearchLoop()
    {
        WaitForSeconds varisKontrol = new WaitForSeconds(0.2f);
        navMeshAgent.speed = enemySearchSpeed;
        while(isSearching)
        {
            yield return varisKontrol;
            if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                OnArrivedSearchPoint?.Invoke();
                yield break;
            }
        }
    }

    void IsPatrolControl(EnemyStates state)
    {
        isPatrol = state == EnemyStates.Patrol;
        if(isPatrol)
        {
           
                loopCoroutine = StartCoroutine(PatrolLoop());
            
        }
        else
        {
           
            if(loopCoroutine != null) { StopCoroutine(loopCoroutine); }
            if(searchCoroutine != null) { StopCoroutine(searchCoroutine); }
            
        }
    }

    void IsSearchingControl(EnemyStates state)
    {
        isSearching = state == EnemyStates.Search;
        if(isSearching)
        {
            if(loopCoroutine != null) { StopCoroutine(loopCoroutine); }
            navMeshAgent.ResetPath();
            navMeshAgent.SetDestination(enemy.PlayerLastLocation);  
            searchCoroutine = StartCoroutine(SearchLoop());
        }
    }

    void IsAlertControl(EnemyStates state)
    {
        isAlert = state == EnemyStates.Alert;
        if(isAlert)
        {
            SetVisualToPlayer();
            if(loopCoroutine != null) { StopCoroutine(loopCoroutine); }
            if(searchCoroutine != null) { StopCoroutine(searchCoroutine); }
            
            navMeshAgent.ResetPath();
        }
    }



    void GoStartingPatrolLocationFirstTime()
    {
        navMeshAgent.SetDestination(patrolTransform[0].position);
    }

    public void SetPatrolLocations(Transform[] transforms)
    {
        patrolTransform = transforms;
    }

    void SetVisualToPlayer()
    {
        Vector3 direction = (playerTransform.position - enemy.transform.position).normalized;
        enemy.transform.forward = direction;
    }

    void ResetEnemy()
    {
        StopAllCoroutines();
        isPatrol = false;
        isArrived = false;
        isSearching = false;
        isAlert = false;
        navMeshAgent.ResetPath();
        navMeshAgent.speed = 0f;
    }
}
