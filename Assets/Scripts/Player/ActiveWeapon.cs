using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }

    private PlayerControls playerControls;

    private float timeBetweenAttacks;

    // used for holding down attack button
    private bool attackButtonDown, isAttacking = false;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();

    }

    private void Start()
    {
        // not passing any parameters to the lambda function
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

        // stops attacking when starting
        AttackCooldown();
    }

    private void Update()
    {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        // when switching weapons, fixes bug where weapons fire immediately
        AttackCooldown();

        timeBetweenAttacks = (CurrentActiveWeapon as InterfaceWeapon).GetWeaponDetails().weaponCooldown;
    }

    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;

    }

    private void AttackCooldown()
    {
        isAttacking = true;
        // stops overlapping attack animations between toggling weapons
        StopAllCoroutines();

        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    private void Attack()
    {
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon)
        {
            AttackCooldown();
            (CurrentActiveWeapon as InterfaceWeapon).Attack();
        }
    }
}
