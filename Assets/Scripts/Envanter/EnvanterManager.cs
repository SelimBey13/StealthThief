using System;
using UnityEngine;

public class EnvanterManager : MonoBehaviour
{
    public static EnvanterManager Instance{get ; private set;}
    [SerializeField] private WeaponData mainPistol;
    [SerializeField] private WeaponData mainKnife;

    WeaponData[] envanter = new WeaponData[2];
    public WeaponData[] Envanter => envanter;
    private void Awake()
    {
        Awaking();
    }

    void Start()
    {
        envanter[0] = mainPistol;
        envanter[1] = mainKnife;
    }
    void Awaking()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    public void SelectPistol(WeaponData pistol)
    {
        envanter[0] = pistol;
    }
    
    public void SelectKnife(WeaponData knife)
    {
        envanter[1] = knife;
    }
    
}
