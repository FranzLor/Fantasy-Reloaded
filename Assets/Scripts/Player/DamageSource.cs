using UnityEngine;

public class DamageSource : MonoBehaviour
{
    private int damageAmount;

    private void Start()
    {
        // change damage amount from weapon details - scriptable object

        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damageAmount = (currentActiveWeapon as InterfaceWeapon).GetWeaponDetails().weaponDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // question mark checks for null ref exception on enemy health
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        enemyHealth?.TakeDamage(damageAmount);
    }
}
