using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 18.0f;
    [SerializeField] private GameObject particlePrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;

    [SerializeField] private float projectileRange = 10.0f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        // check for if not trigger to grab capsule collider in game object
        if (!other.isTrigger && (enemyHealth || indestructible || playerHealth))
        {
            if (playerHealth && isEnemyProjectile)
            {
                playerHealth.TakeDamage(1, transform);
            }

            Instantiate(particlePrefabVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void DetectFireDistance()
    {
        // if projectile is out of range, destroy it
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}
