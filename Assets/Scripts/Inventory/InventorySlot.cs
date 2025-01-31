using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private WeaponDetails weaponDetails;

    public WeaponDetails GetWeaponDetails()
    {
        return weaponDetails;
    }
}
