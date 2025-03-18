using UnityEngine;

public class SpinningBladeProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float range = 10.0f;
    [SerializeField] private int maxBounces = 3;
    [SerializeField] private GameObject projectileHitPrefabVFX;
    [SerializeField] private GameObject projectileExplodeVFX;


    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 movementDirection;
    private int currentBounces = 0;
    private bool isReturning = false;

    private PlayerController playerController;

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("Needs PlayerController");
            return;
        }

        Vector3 playerPosition = playerController.transform.position;
        movementDirection = (playerPosition - transform.position).normalized;

        startPosition = transform.position;
        targetPosition = startPosition + movementDirection * range;
    }

    private void FixedUpdate()
    {
        MoveBlade();
    }

    private void MoveBlade()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (currentBounces >= maxBounces)
            {
                GameObject explosionInstance =
                    Instantiate(projectileExplodeVFX, transform.position, Quaternion.identity);
                Destroy(gameObject);
                Destroy(explosionInstance, 0.4f);
                return;
            }

            if (isReturning)
            {
                targetPosition = startPosition;
            }
            else
            {
                targetPosition = startPosition + movementDirection * range;
            }

            isReturning = !isReturning;
            currentBounces++;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth)
        {
            playerHealth.TakeDamage(1, transform);
            Instantiate(projectileHitPrefabVFX, transform.position, transform.rotation);
        }
    }
}
