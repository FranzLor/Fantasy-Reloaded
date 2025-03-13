using UnityEngine;

public class Phoenix : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject phoenixProjectilePrefab;
    [SerializeField] private GameObject fireSpawnpoint;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    readonly int AttackHash = Animator.StringToHash("Attack");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Attack()
    {
        animator.SetTrigger(AttackHash);

        if (transform.position.x - PlayerController.Instance.transform.position.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    public void SpawnProjectileAnimationEvent()
    {
        if (fireSpawnpoint != null)
        {
            Instantiate(phoenixProjectilePrefab, fireSpawnpoint.transform.position, fireSpawnpoint.transform.rotation);
        }
        else
        {
            Debug.LogWarning("Spawn not set!");
            Instantiate(phoenixProjectilePrefab, transform.position, Quaternion.identity);

        }
    }
}
