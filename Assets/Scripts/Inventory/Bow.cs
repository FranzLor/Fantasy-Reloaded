using UnityEngine;

public class Bow : MonoBehaviour, InterfaceWeapon
{
    public void Attack()
    {
        Debug.Log("Twang");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
