using UnityEngine;

public class TornadoProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float circleSpeed = 100.0f;
    [SerializeField] private int numberOfCircles = 3;
    [SerializeField] private float circleRadius = 1.0f;

    [SerializeField] private float pullForce = 5.0f;
    [SerializeField] private float pullRadius = 5.0f;

    [SerializeField] private float lifetimeDuration = 10.0f;

    private Vector3 targetPosition;
    private bool isCircling = false;
    private float angle = 0.0f;
    private int circlesCompleted = 0;
    private float currentRadius = 0.0f;

    private void Start()
    {
        targetPosition = PlayerController.Instance.transform.position;
        Destroy(gameObject, lifetimeDuration);
    }

    private void Update()
    {
        if (!isCircling)
        {
            MoveTowardTarget();
        }
        else
        {
            CircleAroundTarget();
        }

        ApplyPullEffect();
    }

    private void MoveTowardTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isCircling = true;
        }
    }

    private void CircleAroundTarget()
    {
        // smoothly increase the radius - fixes where teleporting away into circling
        if (currentRadius < circleRadius)
        {
            currentRadius += circleRadius * Time.deltaTime;
        }
        else
        {
            currentRadius = circleRadius;
        }

        angle += circleSpeed * Time.deltaTime;
        if (angle >= 360.0f)
        {
            angle -= 360.0f;
            circlesCompleted++;
        }

        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * currentRadius;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * currentRadius;

        transform.position = targetPosition + new Vector3(x, y, 0);

        if (circlesCompleted >= numberOfCircles)
        {
            Destroy(gameObject);
        }
    }

    private void ApplyPullEffect()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        
        if (distanceToPlayer <= pullRadius)
        {
            Vector2 pullDirection = (transform.position - PlayerController.Instance.transform.position).normalized;

            PlayerController.Instance.GetComponent<Rigidbody2D>().AddForce(pullDirection * pullForce, ForceMode2D.Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth?.TakeDamage(1, transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetPosition, pullRadius);
    }
}