using System;
using UnityEngine;

public class WeaponDirector : MonoBehaviour
{
    public bool weaponSelected{get; private set;}
    public string currentInventoryKey{get; private set;}
    private int index;
    private WeaponData[] selectedEnvanter;
    private WeaponData currentWeapon;



    void Awake()
    {
        weaponSelected = false;
    }

    void Start()
    {
        selectedEnvanter =  EnvanterManager.Instance.Envanter;
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
    }

}
