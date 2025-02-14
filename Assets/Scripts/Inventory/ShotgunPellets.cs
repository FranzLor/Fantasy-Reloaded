using UnityEngine;

public class ShotgunPellets : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 24.0f;
    [SerializeField] private GameObject particlePrefabVFX;

    private WeaponDetails weaponDetails;
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

    public void UpdateWeaponDetails(WeaponDetails weaponDetails)
    {
        this.weaponDetails = weaponDetails;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();

        // check for if not trigger to grab capsule collider in game object
        if (!other.isTrigger && (enemyHealth || indestructible))
        {
            Instantiate(particlePrefabVFX, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }

    private void DetectFireDistance()
    {
        // if projectile is out of range, destroy it
        if (Vector3.Distance(transform.position, startPosition) > weaponDetails.weaponRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}
