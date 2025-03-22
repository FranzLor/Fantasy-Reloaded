using UnityEngine;

public class ForcePushProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float pushStrength = 5.0f;
    [SerializeField] private float lifeTime = 4.0f;
    [SerializeField] private GameObject pushEffectPrefab;

    private Vector2 movementDirection;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Vector2 playerPosition = PlayerController.Instance.transform.position;
        movementDirection = (playerPosition - (Vector2)transform.position).normalized;

        RotateProjectile();

        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        transform.Translate(movementDirection * speed * Time.fixedDeltaTime, Space.World);
    }

    private void RotateProjectile()
    {
        // faces the projectile in the direction of the movement
        float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth)
        {
            playerHealth.TakeDamage(1, transform);

            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
            PlayerController.Instance.ApplyKnockback(knockbackDirection, pushStrength);

            if (pushEffectPrefab != null)
            {
                Instantiate(pushEffectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}