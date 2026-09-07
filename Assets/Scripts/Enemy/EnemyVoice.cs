using System;
using UnityEngine;

public class EnemyVoice : MonoBehaviour
{
    EnemyVision enemyVision;
    Enemy enemy;
    EnemyStateManager enemyStateManager;
    float hearingRadius;
    [SerializeField] float voiceLevel; // 0-100
    float distance;
    float distanceRate;
    float sourceVoice;
    Vector3 direction;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask obstacleLayer;
    public event Action<Vector3> OnHeardSomething; 
    public event Action OnAlert; 
    public event Action<float> OnVoiceLevelChanged; 
    Vector3 suspiciousLocation;

    void Awake()
    {
        enemyVision = GetComponent<EnemyVision>();
        enemy = GetComponent<Enemy>();
        enemyStateManager = GetComponent<EnemyStateManager>();
    }

    void Start()
    {
        hearingRadius = enemyVision.SphereRadius * 0.8f;
    }

    void OnEnable()
    {
        enemy.OnDistanceChanged += SetDistanceToPlayer;
        SourceSoundManager.OnSourceVoiceChanged += SetSourceVoice;
    }
    void OnDisable()
    {
        enemy.OnDistanceChanged -= SetDistanceToPlayer;
        SourceSoundManager.OnSourceVoiceChanged -= SetSourceVoice;
    }

    void SetVoiceLevel()
    {
        voiceLevel = sourceVoice * (1 - distance/hearingRadius);
        voiceLevel = Mathf.Clamp(voiceLevel,0f,100f);
        OnVoiceLevelChanged?.Invoke(voiceLevel);
    }

    void SetDistanceToPlayer(float value)
    {
        distance = value;
        distance = Mathf.Clamp(distance,0f,hearingRadius);
        SetVoiceLevel();
        IsHearingPlayer();
    }
    void IsHearingPlayer()
    {
        if(enemyVision.DotProduct <0f)
        {
            if(voiceLevel > 75f)
            {
                OnAlert?.Invoke();
            }
            else if(voiceLevel > 50 && voiceLevel <75f)
            {
                direction = (playerTransform.position - transform.position).normalized;
                if(Physics.Raycast(transform.position,direction,distance,obstacleLayer))
                {
                    suspiciousLocation = playerTransform.position;
                    OnHeardSomething?.Invoke(suspiciousLocation);
                }
            }
        }
        
    }

    void SetSourceVoice(float value)
    {
        sourceVoice = value;
    }

    


}
