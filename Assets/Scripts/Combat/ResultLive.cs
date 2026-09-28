using System.Collections;
using UnityEngine;

public class ResultLive : MonoBehaviour
{
    [SerializeField] private float liveTime;
    void OnEnable()
    {
        StartCoroutine(CleanUp());
    }

    IEnumerator CleanUp()
    {
        yield return new WaitForSeconds(liveTime);
        gameObject.SetActive(false);
    }
}
