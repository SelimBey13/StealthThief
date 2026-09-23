using UnityEngine;

public class EnemyPlayerReferences : MonoBehaviour
{
    public static EnemyPlayerReferences Instance{get; private set;}
    [SerializeField] public Transform playerTransform;
    [SerializeField] public Transform[] playerBodyTransforms;
    [SerializeField] public PlayerShooting playerShooting;

    void Awake()
    {
        Instance = this;
    }


}
