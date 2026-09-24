using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyStateManager enemyStateManager;
    EnemyVision enemyVision;
    EnemyVoice enemyVoice;
    EnemyMovement enemyMovement;
    [SerializeField] private float moveTime;
    Vector3 playerLastLocation;
    public Vector3 PlayerLastLocation => playerLastLocation;
    [SerializeField] private Transform playerTransform;
    public event Action OnPlayerShot;
    public event Action<float> OnDistanceChanged;
    public event Action OnFirstTimePatrol;
    public event Action<EnemyStates> OnEnemyStateChanged;
    public event Action OnAllCoroutinesMustStop;
    public event Action<Vector3,Rigidbody> OnRagdollPhysic;
    [SerializeField] PlayerShooting playerShooting;
    bool isAlive;
    bool isCleanable;


    void Awake()
    {
        enemyStateManager = GetComponent<EnemyStateManager>();
        enemyVision = GetComponent<EnemyVision>();
        enemyVoice = GetComponent<EnemyVoice>();
        enemyMovement = GetComponent<EnemyMovement>();
        playerShooting = EnemyPlayerReferences.Instance.playerShooting;
    }

    void OnEnable()
    {
        enemyVision.OnEnemyAlert += EnemyAlert;
        enemyVision.OnEnemyShootPlayer += EnemyShootedPlayer;
        enemyVision.OnEnemyEscaped += EnemySearchingPlayer;
        enemyVoice.OnHeardSomething += EnemySearchingPlayer;
        enemyVoice.OnAlert += EnemyAlert;
        enemyMovement.OnArrivedSearchPoint += NothingAroundSearchPoint;
        playerShooting.OnPlayerShootEnemy += EnemyShot;
    }
    void OnDisable()
    {
        enemyVision.OnEnemyAlert -= EnemyAlert;
        enemyVision.OnEnemyShootPlayer -= EnemyShootedPlayer;
        enemyVision.OnEnemyEscaped -= EnemySearchingPlayer;
        enemyVoice.OnHeardSomething -= EnemySearchingPlayer;
        enemyVoice.OnAlert -= EnemyAlert;
        enemyMovement.OnArrivedSearchPoint -= NothingAroundSearchPoint;
        playerShooting.OnPlayerShootEnemy -= EnemyShot;
    }
    void Start()
    {   
        
        isAlive = true;
        isCleanable = false;
        playerTransform = EnemyPlayerReferences.Instance.playerTransform;
        StartCoroutine(WaitForStart(moveTime));
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
            OnDistanceChanged?.Invoke(distance);
            yield return beklemeSuresi;
        }
    }

    IEnumerator WaitForStart(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        enemyStateManager.SetEnemyState(EnemyStates.Patrol);
        OnFirstTimePatrol?.Invoke();
        OnEnemyStateChanged?.Invoke(EnemyStates.Patrol);//işe yaramayacak ama garanti olsun
    }

    void EnemyAlert()
    {
        if(enemyStateManager.CurrentEnemyState() == EnemyStates.Alert){ return;}
        enemyStateManager.SetEnemyState(EnemyStates.Alert);
        enemyVision.SpecifyLastLocation();
        OnEnemyStateChanged?.Invoke(EnemyStates.Alert);
    }
    void EnemyShootedPlayer()
    {
        enemyStateManager.SetEnemyState(EnemyStates.Shoot);
        OnPlayerShot?.Invoke();
        OnEnemyStateChanged?.Invoke(EnemyStates.Shoot);
    }
    void EnemySearchingPlayer(Vector3 lastLocation)
    {
        playerLastLocation = lastLocation;
        enemyStateManager.SetEnemyState(EnemyStates.Search);
        OnEnemyStateChanged?.Invoke(EnemyStates.Search);
    }

    void NothingAroundSearchPoint()
    {
        enemyStateManager.SetEnemyState(EnemyStates.Patrol);
        OnEnemyStateChanged?.Invoke(EnemyStates.Patrol);
    }

    public void SetPatrolTransforms(Transform[] transforms)
    {
        enemyMovement.SetPatrolLocations(transforms);
    }

    void Die()
    {
        if(!isAlive) {return;}

        enemyStateManager.SetEnemyState(EnemyStates.Dead);
        OnEnemyStateChanged?.Invoke(EnemyStates.Dead);
        OnAllCoroutinesMustStop?.Invoke();
        StopAllCoroutines();
        isAlive = false;
        OnRagdollPhysic?.Invoke(playerShooting.FireDirection , playerShooting.FiredRigidbody);


        Invoke(nameof(SetCleanable),3f); // öldükten 3 saniye sonra ölü bedeni temizleyebilme mekaniği
    }

    void SetCleanable()
    {
        isCleanable = true;
    }

    void EnemyShot(Enemy enemy)
    {
        if(enemy == this)
        {
            Die();
        }
    }

}
