using UnityEngine;

[CreateAssetMenu(menuName = "New Weapon")]
public class WeaponDetails : ScriptableObject
{
    public GameObject weaponPrefab;

    public float weaponCooldown;
    public float weaponRange;
    public int weaponDamage;

}
