using UnityEngine;

[CreateAssetMenu]
public class SceneData : ScriptableObject
{
    [SerializeField] private SceneType sceneType;
    [SerializeField] private string sceneName;
    [SerializeField] private Sprite mapThumbnail; // map giris butonlarına mapin görseli koyulacak
    [SerializeField] private int enemyCount;

    public int EnemyCount => enemyCount;
    
}
