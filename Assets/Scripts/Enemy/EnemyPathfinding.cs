using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2.0f;

    private Rigidbody2D rigidBody;
    private Vector2 moveDirection;
    EnemyAI enemyAI;

    private Knockback knockback;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        enemyAI = GetComponent<EnemyAI>();
        knockback = GetComponent<Knockback>();
    }

    private void FixedUpdate()
    {
        if (knockback.gettingKnockedBack)
        {
            return;
        }

        rigidBody.MovePosition(rigidBody.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));
    }

    public void MoveTo(Vector2 targetPosition)
    {
        moveDirection = targetPosition;
    }
}