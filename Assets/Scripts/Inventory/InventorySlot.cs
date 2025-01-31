using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private WeaponDetails weaponDetails;

    private WeaponDetails GetWeaponDetails()
    {
        return weaponDetails;
    }
}
