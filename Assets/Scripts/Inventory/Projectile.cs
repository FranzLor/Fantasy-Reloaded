using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 18.0f;
    [SerializeField] private GameObject particlePrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10.0f;
    [SerializeField] public bool useDynamicDirection = false;

    private Vector3 startPosition;
    private Vector3 direction = Vector3.right;

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

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void SetDirection(Vector3 newDirection)
    {
        if (useDynamicDirection)
        {
            direction = newDirection.normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        // check for if not trigger to grab capsule collider in game object
        if (!other.isTrigger && (enemyHealth || indestructible || playerHealth))
        {
            if ((playerHealth && isEnemyProjectile) || (enemyHealth && !isEnemyProjectile))
            {
                playerHealth?.TakeDamage(1, transform);

                Instantiate(particlePrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            // fix for indestructible objects, will need to refactor later
            else if (!other.isTrigger && indestructible)
            {
                Instantiate(particlePrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
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
        transform.Translate(direction * Time.deltaTime * moveSpeed);
    }
}
