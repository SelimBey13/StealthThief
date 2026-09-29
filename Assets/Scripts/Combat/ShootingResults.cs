using System.Collections.Generic;
using UnityEngine;

public class ShootingResults : MonoBehaviour
{
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private GameObject holeDecalPrefab;
    [SerializeField] private GameObject bloodDecalPrefab;
    [SerializeField] private int maxHole;
    [SerializeField] private int maxBlood;
    private int currentHole;
    private int currentBlood;
    List<GameObject> holeList = new List<GameObject>();
    private Vector3 bulletDirection;
    [SerializeField] private GameObject particle;

    void OnEnable()
    {
        playerShooting.OnHitInfos += HitInfo;
    }
    void OnDisable()
    {
        playerShooting.OnHitInfos -= HitInfo;
    }

    void Start()
    {
        StartingLists();
    }

    void HitInfo(RaycastHit hit , Vector3 vector3)
    {
        bulletDirection = vector3;
        if(hit.collider.gameObject.layer == LayerMask.NameToLayer("SolidArticle"))
        {
            currentHole ++;
            Quaternion rotation = Quaternion.LookRotation(-hit.normal);
            Vector3 hitPoint = hit.point + (hit.normal * 0.05f);
            holeList[currentHole%maxHole].SetActive(false);
            holeList[currentHole%maxHole].transform.position = hitPoint;
            holeList[currentHole%maxHole].transform.rotation = rotation;
            holeList[currentHole%maxHole].SetActive(true);
        }
        else if(hit.collider.GetComponentInParent<Enemy>())
        {
            GameObject liveParticle;
            liveParticle = Instantiate(particle,hit.point,Quaternion.LookRotation(bulletDirection));
        }
    }

    void StartingLists()
    {
        for(int i=0; i<maxHole; i++)
        {
            GameObject liveHole = Instantiate(holeDecalPrefab);
            holeList.Add(liveHole);
            liveHole.SetActive(false);
        }
    }

}
