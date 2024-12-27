using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField]
    private float knockbackTime = 0.15f;

    private Rigidbody2D rigidBody;

    public bool gettingKnockedBack { get; private set; }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource, float knockbackThrust)
    {
        gettingKnockedBack = true;
        // gets the direction from the enemy to the player, normalizes it, and multiplies it by the knockback thrust and the mass of the rigidbody
        Vector2 difference = (transform.position - damageSource.position).normalized * knockbackThrust * rigidBody.mass;
        rigidBody.AddForce(difference, ForceMode2D.Impulse);

        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockbackTime);

        rigidBody.linearVelocity = Vector2.zero;
        gettingKnockedBack = false;
    }
}
