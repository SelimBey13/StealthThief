using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    [SerializeField] string enemyName;
    [SerializeField] float enemyPatrolSpeed; // genel gezme hızı
    [SerializeField] float enemySearchSpeed;
    [SerializeField] float enemyPatienceTime; // kac saniye sonra vuracak
    public string EnemyName => enemyName;
    public float EnemyWanderSpeed => enemyPatrolSpeed;
    public float EnemySearchSpeed => enemySearchSpeed;
    public float EnemyPatienceTime => enemyPatienceTime;

    //gerekirse can sistemi eklenebilir
}
