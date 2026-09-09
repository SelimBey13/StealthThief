using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance{get; private set;}
    private int enemyCount;
    public int EnemyCount => enemyCount;
    void Awake()
    {
        AwakingController();

    }

    void AwakingController()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance=this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveSceneInformations(SceneData sceneData)
    {
        enemyCount = sceneData.EnemyCount;

    }
}
