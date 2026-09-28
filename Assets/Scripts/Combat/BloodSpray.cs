using System.Collections.Generic;
using UnityEngine;

public class BloodSpray : MonoBehaviour
{
    List<GameObject> currentBloodList = new List<GameObject>();

    void OnEnable()
    {
        
    }
    void OnDisable()
    {
        
    }

    void SetCurrentList(List<GameObject> list)
    {
        currentBloodList = list;
    }
}
