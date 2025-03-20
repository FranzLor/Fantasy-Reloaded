using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirectionFloat = 2.0f;
    [SerializeField] private float attackRange = 0.0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 1.4f;
    [SerializeField] private bool stopMovingDuringAttack = false;
    [SerializeField] private float chaseRange = 5.0f;

    private State state;
    private EnemyPathfinding enemyPathfinding;
    private Vector2 roamPosition;
    private float timeRoaming = 0.0f;
    private bool canAttack = true;

    private enum State
    {
        // TODO: more states, chasing, attacking, etc...
        Roaming,
        Chasing,
        Attacking
    }


    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        switch (state)
        {
            case State.Roaming:
                Roaming();
                if (distanceToPlayer < chaseRange)
                {
                    state = State.Chasing;
                }
                break;

            case State.Chasing:
                Chasing();
                if (distanceToPlayer < attackRange)
                {
                    state = State.Attacking;
                }
                else if (distanceToPlayer > chaseRange)
                {
                    state = State.Roaming;
                }
                break;

            case State.Attacking:
                Attacking();
                if (distanceToPlayer > attackRange)
                {
                    state = State.Chasing;
                }
                break;
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;

        enemyPathfinding.MoveTo(roamPosition);

        if (timeRoaming > roamChangeDirectionFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Chasing()
    {
        Vector2 playerPosition = PlayerController.Instance.transform.position;
        enemyPathfinding.MoveTo((playerPosition - (Vector2)transform.position).normalized);
    }

    private void Attacking()
    {
        if (attackRange != 0 && canAttack)
        {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopMovingDuringAttack)
            {
                enemyPathfinding.StopMoving();
            }
            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0.0f;
        // normalized to prevent diagonal movement being faster
        return new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
    }

    private void OnDrawGizmosSelected()
    {
        // KEEP FOR NOW
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
