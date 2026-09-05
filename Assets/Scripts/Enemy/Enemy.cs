using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyStateManager enemyStateManager;
    EnemyVision enemyVision;
    EnemyVoice enemyVoice;
    [SerializeField] private float moveTime;
    Vector3 playerLastLocation;
    [SerializeField] private Transform playerTransform;
    public event Action OnPlayerShot;
    public event Action<float> OnDistanceChanged;

    void Awake()
    {
        enemyStateManager = GetComponent<EnemyStateManager>();
        enemyVision = GetComponent<EnemyVision>();
        enemyVoice = GetComponent<EnemyVoice>();
    }

    void OnEnable()
    {
        enemyVision.OnEnemyAlert += EnemyAlert;
        enemyVision.OnEnemyShootPlayer += EnemyShootedPlayer;
        enemyVision.OnEnemyEscaped += EnemySearchingPlayer;
        enemyVoice.OnHeardSomething += EnemySearchingPlayer;
        enemyVoice.OnAlert += EnemyAlert;
    }
    void OnDisable()
    {
        enemyVision.OnEnemyAlert -= EnemyAlert;
        enemyVision.OnEnemyShootPlayer -= EnemyShootedPlayer;
        enemyVision.OnEnemyEscaped -= EnemySearchingPlayer;
        enemyVoice.OnHeardSomething -= EnemySearchingPlayer;
        enemyVoice.OnAlert -= EnemyAlert;
    }
    void Start()
    {
        StartCoroutine(WaitForStart(moveTime));
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
    }

    void EnemyAlert()
    {
        enemyStateManager.SetEnemyState(EnemyStates.Alert);
    }
    void EnemyShootedPlayer()
    {
        enemyStateManager.SetEnemyState(EnemyStates.Shoot);
        OnPlayerShot?.Invoke();
    }
    void EnemySearchingPlayer(Vector3 lastLocation)
    {
        playerLastLocation = lastLocation;
        enemyStateManager.SetEnemyState(EnemyStates.Search);
    }

}
