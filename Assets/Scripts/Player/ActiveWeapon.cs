using UnityEngine;
using UnityEngine.Rendering;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    [SerializeField] private MonoBehaviour currentActiveWeapon;

    private PlayerControls playerControls;

    // used for holding down attk button
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
    }

    private void Update()
    {
        Attack();
    }

    public void ToggleIsAttacking(bool value)
    {
        isAttacking = value;
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

    private void Attack()
    {
        if (attackButtonDown && !isAttacking)
        {
            isAttacking = true;
            (currentActiveWeapon as InterfaceWeapon).Attack();
        }
    }
}
