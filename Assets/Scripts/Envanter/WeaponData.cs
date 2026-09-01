using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private string weaponName;
    [SerializeField] private int weaponDamage;
    [SerializeField] private GameObject weaponPrefab;

    public WeaponType WeaponType => weaponType;
    public string WeaponName => weaponName;
    public int WeaponDamage => weaponDamage;
    public GameObject WeaponPrefab => weaponPrefab;

}
