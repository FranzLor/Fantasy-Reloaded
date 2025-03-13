using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3.0f;

    private Rigidbody2D rigidBody;
    private Vector2 moveDirection;
    EnemyAI enemyAI;

    private Knockback knockback;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        enemyAI = GetComponent<EnemyAI>();
        knockback = GetComponent<Knockback>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (knockback.GettingKnockedBack)
        {
            return;
        }

        rigidBody.MovePosition(rigidBody.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));

        if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void MoveTo(Vector2 targetPosition)
    {
        moveDirection = targetPosition;
    }

    public void StopMoving()
    {
        moveDirection = Vector2.zero;
    }
}
