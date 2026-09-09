using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PatrolGroup
{   
        public Transform[] points;
}
public class PatrolTransformManager : MonoBehaviour
{
    public static PatrolTransformManager Instance{get; private set;}

    void Awake()
    {
        Instance = this;
    }

    [SerializeField] public List<PatrolGroup> patrolTransforms = new List<PatrolGroup>();

}
