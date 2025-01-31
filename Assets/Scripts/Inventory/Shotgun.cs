using UnityEngine;

public class Shotgun : MonoBehaviour, InterfaceWeapon
{
    public void Attack()
    {
        Debug.Log("POW");
        ActiveWeapon.Instance.ToggleIsAttacking(false);

    }
}
