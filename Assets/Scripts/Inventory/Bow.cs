using UnityEngine;

public class Bow : MonoBehaviour, InterfaceWeapon
{
    [SerializeField] private WeaponDetails weaponDetails;

    public WeaponDetails GetWeaponDetails()
    {
        return weaponDetails;
    }

    public void Attack()
    {
        Debug.Log("Twang");
    }
}
