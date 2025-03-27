using UnityEngine;

public class DirectedProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15.0f;
    [SerializeField] private float range = 10.0f;
    [SerializeField] private GameObject impactVFX;
    [SerializeField] private bool isEnemyProjectile = true;

    [SerializeField] private bool faceMovementDirection = true;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector2 direction;
    private Vector2 spawnPosition;
    private Transform playerTransform;
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerTransform = PlayerController.Instance.transform;
        spawnPosition = transform.position;
    }

    private void Start()
    {
        direction = (playerTransform.position - transform.position).normalized;
        rigidBody.linearVelocity = direction * moveSpeed;

        if (faceMovementDirection)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (spriteRenderer != null)
        {
            spriteRenderer.flipX = direction.x < 0;
        }
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(spawnPosition, transform.position) >= range)
        {
            DestroyProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger) return;

        // Damage logic (same as your original)
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.GetComponent<Indestructible>();

        if (playerHealth && isEnemyProjectile)
        {
            playerHealth.TakeDamage(1, transform);
            DestroyProjectile();
        }
        else if (enemyHealth && !isEnemyProjectile)
        {
            enemyHealth.TakeDamage(1);
            DestroyProjectile();
        }
        else if (indestructible)
        {
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        if (impactVFX != null)
        {
            Instantiate(impactVFX, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
