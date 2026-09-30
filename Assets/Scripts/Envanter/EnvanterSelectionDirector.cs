using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class WeaponButton
{
    public Outline outline;
    public WeaponData weaponData;
}

public class EnvanterSelectionDirector : MonoBehaviour
{
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private List<WeaponButton> pistolButtons = new List<WeaponButton>();
    [SerializeField] private List<WeaponButton> knifeButtons = new List<WeaponButton>();

    public void RefreshHighlights()
    {
        WeaponData[] envanter = EnvanterManager.Instance.Envanter;
        Highlight(pistolButtons, envanter[0]);
        Highlight(knifeButtons, envanter[1]);
    }

    void Start()
    {
        RefreshHighlights();
    }

    public void SelectPistol(WeaponData weaponData)
    {
        EnvanterManager.Instance.SelectPistol(weaponData);
        Highlight(pistolButtons, weaponData);
    }

    public void SelectKnife(WeaponData weaponData)
    {
        EnvanterManager.Instance.SelectKnife(weaponData);
        Highlight(knifeButtons, weaponData);
    }

    void Highlight(List<WeaponButton> buttons, WeaponData selected)
    {
        foreach (WeaponButton weaponButton in buttons)
        {
            weaponButton.outline.effectColor = (weaponButton.weaponData == selected) ? selectedColor : normalColor;
        }
    }
}