using System.Collections;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private Collider2D meleeCollider;
    [SerializeField] private float attackDuration = 1.0f;
    [SerializeField] private Transform weaponCollider;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isAttacking = false;
    
    readonly int attackHash = Animator.StringToHash("Attack");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        meleeCollider.enabled = false;

    }

    public void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger(attackHash);

        bool facingLeft = transform.position.x - PlayerController.Instance.transform.position.x > 0;

        FlipEnemyAndCollider(facingLeft);
    }

    public void EnabledColliderForAttack()
    {
        meleeCollider.enabled = true;
        StartCoroutine(DisableCollider());
    }

    private IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(attackDuration);
        meleeCollider.enabled = false;
        isAttacking = false;
    }

    private void FlipEnemyAndCollider(bool facingLeft)
    {
        spriteRenderer.flipX = facingLeft;

        if (facingLeft)
        {
            weaponCollider.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            weaponCollider.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    
}
