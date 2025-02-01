using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, InterfaceWeapon
{
    [SerializeField] private GameObject slashAnimationPrefab;

    // DO NOT SET TOO SHORT, creates weapon collider bug where it never toggles
    [SerializeField] private float attackCooldown = 0.5f;

    [SerializeField] private WeaponDetails weaponDetails;

    private Transform weaponCollider;
    private Transform slashAnimationSpawnpoint;
    private Animator animator;
    private GameObject slashAnimation = null;

    private void Awake()
    {
        // using getcomponentinparent since both are single classes | one instance 
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public WeaponDetails GetWeaponDetails()
    {
        return weaponDetails;
    }

    public void Attack()
    {
        // isAttacking = true;

        // play attack animation
        animator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);

        slashAnimation = Instantiate(slashAnimationPrefab, slashAnimationSpawnpoint.position, Quaternion.identity);
        slashAnimation.transform.parent = this.transform.parent;
    }

    private void Start()
    {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimationSpawnpoint = PlayerController.Instance.GetSlashAnimationSpawnpoint();
    }

    public void FinishAttackAnimationEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimationEvent()
    {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (PlayerController.Instance.FacingLeft)
        {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimationEvent()
    {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (PlayerController.Instance.FacingLeft)
        {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        // multiply by rad2deg to convert to degrees
        // used for mouse follow rotation
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
    }
}
