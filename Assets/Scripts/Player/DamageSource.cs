using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 50;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // question mark checks for null ref exception on enemy health
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        enemyHealth?.TakeDamage(damageAmount);
    }
}
