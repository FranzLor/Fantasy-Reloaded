using UnityEngine;

[CreateAssetMenu(menuName = "New Weapon")]
class WeaponDetails : ScriptableObject
{
    public GameObject weaponPrefab;

    public float weaponCooldown;
}
