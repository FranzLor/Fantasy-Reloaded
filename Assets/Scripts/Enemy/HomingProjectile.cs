using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    [SerializeField] private float homingSpeed = 5.0f;
    [SerializeField] private float rotateSpeed = 200.0f;
    [SerializeField] private Transform shadow;
    [SerializeField] private GameObject particlePrefabVFX;


    private Transform target;
    private PlayerController playerController;
    private Rigidbody2D rigidBody;


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        target = PlayerController.Instance.transform;
    }

    private void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rigidBody.position;
        // changes length of vector to 1 without having to change direction
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.right).z;

        rigidBody.angularVelocity = -rotateAmount * rotateSpeed;
        rigidBody.linearVelocity = transform.right * homingSpeed;


        if (shadow != null)
        {
            shadow.position = transform.position + Vector3.down * 0.9f;
            shadow.rotation = Quaternion.identity;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth)
        {
            playerHealth.TakeDamage(1, transform);
            Instantiate(particlePrefabVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
