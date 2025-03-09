using System.Collections;
using UnityEngine;

public class BulletShooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField] [Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float restTime = 1.0f;
    [SerializeField] private bool oscillate;
    [SerializeField] private bool stagger;

    private bool isShooting = false;

    public void Attack()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }

        
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;
        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0.0f;

        TargetCone(out startAngle, out currentAngle, out angleStep, out endAngle);

        if (stagger)
        {
            timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst;
        }

        for (int i = 0; i < burstCount; i++)
        {
            if (!oscillate)
            {
                TargetCone(out startAngle, out currentAngle, out angleStep, out endAngle);
            }

            // fixes problem where oscillate bursts dont point towards player, when both staggered and oscillate are true
            if (oscillate && i % 2 != 1)
            {
                TargetCone(out startAngle, out currentAngle, out angleStep, out endAngle);

            }
            else if (oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }


            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 position = FindBulletSpawnPosition(currentAngle);

                GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
                bullet.transform.right = bullet.transform.position - transform.position;

                if (bullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;

                if (stagger)
                {
                    yield return new WaitForSeconds(timeBetweenProjectiles);
                }
            }

            currentAngle = startAngle;

            // removes wait time when oscillating, infinite shooting
            if (!stagger)
            {
                yield return new WaitForSeconds(timeBetweenBursts);
            }
        }

        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    private void TargetCone(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;

        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0.0f;
        angleStep = 0.0f;

        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2.0f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPosition(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 position = new Vector2(x, y);

        return position;
    }
}
