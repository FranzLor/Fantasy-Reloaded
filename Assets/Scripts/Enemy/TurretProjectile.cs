using System.Collections;
using TMPro;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10.0f;
    [SerializeField] private int burstCount = 3;
    [SerializeField] private int shotsPerBurst = 3;
    [SerializeField] private float delayBetweenShots = 1.0f;
    [SerializeField] private float delayBetweenBursts = 2.0f;

    [SerializeField] private bool hasLifetime = true;
    [SerializeField] private float lifetimeDuration = 10.0f;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnpoint;

    private bool isShooting = false;
    private int currentBursts = 0;

    private void Start()
    {
        if (IsPlayerInRange())
        {
            StartShooting();
        }

        if (hasLifetime)
        {
            Destroy(gameObject, lifetimeDuration);
        }
    }

    private void Update()
    {
        if (!isShooting && IsPlayerInRange())
        {
            StartShooting();
        }
    }

    private bool IsPlayerInRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        return distanceToPlayer <= detectionRadius;
    }

    private void StartShooting()
    {
        if (!isShooting)
        {
            isShooting = true;
            currentBursts = 0;
            StartCoroutine(ShootBursts());
        }
    }

    private IEnumerator ShootBursts()
    {
        while (currentBursts < burstCount)
        {
            for (int i = 0; i < shotsPerBurst; i++)
            {
                ShootProjectile();
                yield return new WaitForSeconds(delayBetweenShots);
            }

            currentBursts++;
            yield return new WaitForSeconds(delayBetweenBursts);
        }

        isShooting = false;
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Add Projectiles for Turret.");
            return;
        }

        Vector3 direction = 
            (PlayerController.Instance.transform.position -
             (projectileSpawnpoint ? projectileSpawnpoint.position : transform.position)).normalized;

        GameObject projectile = 
            Instantiate(projectilePrefab, projectileSpawnpoint ? projectileSpawnpoint.position : transform.position, Quaternion.identity);

        if (projectile.TryGetComponent(out Projectile projectileComponent))
        {
            projectileComponent.SetDirection(direction);
            projectileComponent.useDynamicDirection = true;
        }
        else
        {
            Debug.LogError("Turret's Projectile needs a projectile component.");
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
