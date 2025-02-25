using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float moveSpeed = 4.0f;

    [SerializeField] private float dashSpeed = 2.0f;

    [SerializeField] private float dashTime = 0.18f;

    [SerializeField] private TrailRenderer trailRenderer;

    [SerializeField] private float dashCooldown = 1.0f;

    [SerializeField] private Transform weaponCollider;
    [SerializeField] private Transform slashAnimationSpawnpoint;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rigidBody;
    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    private Knockback knockback;
    private float startingMoveSpeed;

    private bool isDashing = false;
    private bool isKnockbackActive = false;

    // used for slash animation
    private bool facingLeft = false;
    // getter and setter for facingLeft
    public bool FacingLeft { get { return facingLeft; } }

    protected override void Awake()
    {
        // called from singleton script
        base.Awake();

        playerControls = new PlayerControls();
        rigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();

        startingMoveSpeed = moveSpeed;
    }


    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    public Transform GetSlashAnimationSpawnpoint()
    {
        return slashAnimationSpawnpoint;
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        // might need to update for shotgun knockback
        // if enemy knockback, turns off movement to apply knockback
        if (knockback.gettingKnockedBack)
        {
            return;
        }

        //rigidBody.MovePosition(rigidBody.position + movement * (moveSpeed * Time.fixedDeltaTime));
        if (isKnockbackActive)
        {

        }
        else
        {
            rigidBody.MovePosition(rigidBody.position + movement * (moveSpeed * Time.fixedDeltaTime));
        }

        //Vector2 moveForce = movement * moveSpeed;
        // rigidBody.AddForce(moveForce, ForceMode2D.Force);
    }

    public void ApplyKnockback(Vector2 knockbackForce)
    {
        isKnockbackActive = true;
        //Vector2 moveForce = movement * moveSpeed;
        rigidBody.AddForce(knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(ResetKnockback());
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.2f);
        isKnockbackActive = false;
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            spriteRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            spriteRenderer.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            moveSpeed *= dashSpeed;
            trailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        trailRenderer.emitting = false;

        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
    }
}
