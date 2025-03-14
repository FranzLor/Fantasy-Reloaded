using UnityEngine;

public class ProjectileLandSplatter : MonoBehaviour
{
    [SerializeField] private float disableColliderTime = 1.0f;

    private Transparency transparency;

    private void Awake()
    {
        transparency = GetComponent<Transparency>();
    }

    private void Start()
    {
        StartCoroutine(transparency.SlowFade());

        Invoke("DisableCollider", disableColliderTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // edit later
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(1, transform);
    }

    private void DisableCollider()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
