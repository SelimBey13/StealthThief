using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform[] patrolTransform;
    EnemyStateManager enemyStateManager;
    Enemy enemy;
    EnemyVision enemyVision;
    EnemyVoice enemyVoice;
    NavMeshAgent navMeshAgent;
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
        currentPatrol = 0;
        direction = 1;
    }

    void OnEnable()
    {
        enemy.OnFirstTimePatrol += GoStartingPatrolLocationFirstTime;
        enemy.OnEnemyStateChanged += IsPatrolControl;
        enemy.OnEnemyStateChanged += IsSearchingControl;
        enemy.OnEnemyStateChanged += IsAlertControl;
    }

    void OnDisable()
    {
        enemy.OnFirstTimePatrol -= GoStartingPatrolLocationFirstTime;
        enemy.OnEnemyStateChanged -= IsPatrolControl;
        enemy.OnEnemyStateChanged -= IsSearchingControl;
        enemy.OnEnemyStateChanged -= IsAlertControl;
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
        while(isPatrol)
        {
            yield return varisKontrol;
            IsArrived();
        }
    }

    IEnumerator SearchLoop()
    {
        WaitForSeconds varisKontrol = new WaitForSeconds(0.2f);
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
}
