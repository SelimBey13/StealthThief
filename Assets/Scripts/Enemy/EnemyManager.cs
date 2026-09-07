using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance{get;private set;}
    [SerializeField] private GameObject[] enemyPrefab;
    List<Enemy> enemies = new List<Enemy>();

    

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        loadToListAndInstantiate();
    }

    void loadToListAndInstantiate()
    {
        for(int i=0; i<1; i++) // 5 -> SceneManager.Instance.CurrentScene.enemyCount
        {
            int rand = 0;
            GameObject enemyLive;
            Enemy enemy;
            rand = Random.Range(0,enemyPrefab.Length);
            enemyLive = Instantiate(enemyPrefab[rand]);
            enemy = enemyLive.GetComponent<Enemy>();
            enemies.Add(enemy); 
        }
    }
}
