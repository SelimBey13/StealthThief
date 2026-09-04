using System;
using UnityEngine;

public class WeaponDirector : MonoBehaviour
{
    public static WeaponDirector Instance{get; private set;}
    public bool weaponSelected{get; private set;}
    public string currentInventoryKey{get; private set;}
    private int index;
    public int Index => index;
    private WeaponData[] selectedEnvanter;
    private WeaponData currentWeapon;
    [SerializeField] private Transform pistolHandTransorm;
    [SerializeField] private Transform pistolFreeTransform;
    [SerializeField] private Transform knifeHandTransorm;
    [SerializeField] private Transform knifeFreeTransform;
    private WeaponData gamePistolData;
    private WeaponData gameKnifeData;
    GameObject gamePistol;
    GameObject gameKnife;



    void Awake()
    {
        weaponSelected = false;
        Instance = this;
    }

    void Start()
    {
        selectedEnvanter =  EnvanterManager.Instance.Envanter;
        gamePistolData = selectedEnvanter[0];
        gameKnifeData = selectedEnvanter[1];
        gamePistol = Instantiate(gamePistolData.WeaponPrefab,pistolFreeTransform);
        gameKnife = Instantiate(gameKnifeData.WeaponPrefab,knifeFreeTransform);

        gamePistol.SetActive(true);
        gameKnife.SetActive(true);

    }
    void OnEnable()
    {
        InputManager.OnMouseInventory += UpdateCurrentInventoryKey;
        InputManager.OnGamepadInventory += UpdateCurrentInventoryKey;
    }

    void OnDisable()
    {
        InputManager.OnMouseInventory -= UpdateCurrentInventoryKey;
        InputManager.OnGamepadInventory -= UpdateCurrentInventoryKey;
    }

    void UpdateCurrentInventoryKey(String key)
    {
        if(InputManager.CurrentActiveDevice == ActiveDevice.Mouse)
        {
                if(currentInventoryKey != key)
                {
                    currentInventoryKey = key;
                    weaponSelected = true;
                    if(key =="1")
                    {   
                        index = 0;
                    }
                    else if(key =="2")
                    {
                        index = 1;
                    }
                }
                else if(currentInventoryKey == key)
                {
                    weaponSelected = false;
                    currentInventoryKey = null;
                }    
        }
        else if(InputManager.CurrentActiveDevice == ActiveDevice.Gamepad)
        {
 
            if(key == "up")
            {
                weaponSelected = true;
                currentInventoryKey = key;
                index = 0;
            }
            else if(key == "right")
            {
                weaponSelected = true;
                currentInventoryKey = key;
                index = 1;
            }
            else if(key == "down")
            {
                weaponSelected = false;
                currentInventoryKey = key;
            }

        }

        SetWeapon();
        
    }

    void SetWeapon()
    {
        if(weaponSelected)
        {
            currentWeapon = selectedEnvanter[index];
        }
        else
        {
            currentWeapon = null;
        }

        SetWeaponVisual();
    }

    void SetWeaponVisual()
    {
        if(currentWeapon == null)
        {
            gameKnife.transform.SetParent(knifeFreeTransform);
            ResetTransform(gameKnife);
            gamePistol.transform.SetParent(pistolFreeTransform);
            ResetTransform(gamePistol);
            return;
        }

        if(currentWeapon == gamePistolData)
        {
            gameKnife.transform.SetParent(knifeFreeTransform);
            ResetTransform(gameKnife);
            gamePistol.transform.SetParent(pistolHandTransorm);
            ResetTransform(gamePistol);
        }
        else if(currentWeapon == gameKnifeData)
        {
            gamePistol.transform.SetParent(pistolFreeTransform);
            ResetTransform(gamePistol);
            gameKnife.transform.SetParent(knifeHandTransorm);
            ResetTransform(gameKnife);
        }
    }

    void ResetTransform(GameObject obj)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }
}
