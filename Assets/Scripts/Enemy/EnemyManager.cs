using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance{get;private set;}
    [SerializeField] private GameObject[] enemyPrefab;
    List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private PlayerShooting playerShooting;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        loadToListAndInstantiate();
    }
    void OnEnable()
    {
        playerShooting.OnPlayerShootEnemy += EnemyShot;
    }
    void OnDisable()
    {
        playerShooting.OnPlayerShootEnemy -= EnemyShot;
    }

    void loadToListAndInstantiate()
    {
        for(int i=0; i<GameSceneManager.Instance.EnemyCount; i++)
        {
            int rand = 0;
            GameObject enemyLive;
            Enemy enemy;
            rand = Random.Range(0,enemyPrefab.Length);
            enemyLive = Instantiate(enemyPrefab[rand],PatrolTransformManager.Instance.patrolTransforms[i].points[0].position,Quaternion.identity);
            enemy = enemyLive.GetComponent<Enemy>();
            enemies.Add(enemy); 
            enemy.SetPatrolTransforms(PatrolTransformManager.Instance.patrolTransforms[i].points);
        }
    }

    void EnemyShot(Enemy enemy)
    {
        if(enemies.Contains(enemy))
        {
          enemies.Remove(enemy);  
        }
        
    }

}
