using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, InterfaceWeapon
{
    [SerializeField] 
    private GameObject slashAnimationPrefab;

    [SerializeField] 
    private Transform slashAnimationSpawnpoint;

    [SerializeField] 
    private Transform weaponCollider;

    // DONT SET TOO SHORT, creates weapon collider bug where it never toggles
    [SerializeField]
    private float attackCooldown = 0.5f;

    private Animator animator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;

    private GameObject slashAnimation = null;

    private void Awake()
    {
        // using getcomponentinparent since both are single classes | one instance 
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        // isAttacking = true;

        // play attack animation
        animator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);

        slashAnimation = Instantiate(slashAnimationPrefab, slashAnimationSpawnpoint.position, Quaternion.identity);
        slashAnimation.transform.parent = this.transform.parent;

        StartCoroutine(AttackCooldownRoutine());

    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        ActiveWeapon.Instance.ToggleIsAttacking(false);

    }

    public void FinishAttackAnimationEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimationEvent()
    {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (playerController.FacingLeft)
        {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimationEvent()
    {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (playerController.FacingLeft)
        {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        // multiply by rad2deg to convert to degrees
        // used for mouse follow rotation
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
    }
}
