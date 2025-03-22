using UnityEngine;

public class Mage : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject mageProjectilePrefab;
    [SerializeField] private GameObject orbitalProjectilePrefab;

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
        Instantiate(mageProjectilePrefab, transform.position, Quaternion.identity);
    }

    public void SpawnOrbitalProjectileAnimationEvent()
    {
        // used by orbital projectiles
        // separate from mage projectile - this will parent obj to the mage - might not need for other projectiles
        Instantiate(orbitalProjectilePrefab, transform.position, Quaternion.identity, transform);
    }
}