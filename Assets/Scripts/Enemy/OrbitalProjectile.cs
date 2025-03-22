using UnityEngine;

public class OrbitalProjectile : MonoBehaviour
{
    [SerializeField] private float orbitalSpeed = 200.0f;
    [SerializeField] private float orbitalRadius = 2.0f;
    [SerializeField] private float orbitalVerticalOffset = -1.0f;
    [SerializeField] private float lifetimeDuration = 10.0f;
    [SerializeField] private GameObject projectileHitPrefabVFX;

    private Transform mageTransform;
    private float angle = 0.0f;

    private void Start()
    {
        // being parented is important, use orbital projectile serialized field instead of mageprojectile
        mageTransform = transform.parent;

        if (mageTransform == null)
        {
            return;
        }

        transform.position = GetOrbitalPosition(angle);

        Destroy(gameObject, lifetimeDuration);
    }

    private void Update()
    {
        OrbitAroundMage();
    }

    private void OrbitAroundMage()
    {
        if (mageTransform == null)
        {
            return;
        }

        angle += orbitalSpeed * Time.deltaTime;
        if (angle >= 360.0f) angle -= 360.0f;

        transform.position = GetOrbitalPosition(angle);
    }

    private Vector3 GetOrbitalPosition(float currentAngle)
    {
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * orbitalRadius;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * orbitalRadius;

        Vector3 magePosition = mageTransform.position + new Vector3(0, orbitalVerticalOffset, 0);

        return magePosition + new Vector3(x, y, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth?.TakeDamage(1, transform);
            Destroy(gameObject);

            if (projectileHitPrefabVFX != null)
            {
                GameObject explosionInstance = Instantiate(projectileHitPrefabVFX, transform.position, Quaternion.identity);
                Destroy(explosionInstance, 0.4f);
            }
        }
    }
}
