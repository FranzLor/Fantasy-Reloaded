using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float knockbackTime = 0.15f;

    private Rigidbody2D rigidBody;
    private bool gettingKnockedBack = false;

    public bool GettingKnockedBack => gettingKnockedBack;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource, float knockbackThrust)
    {
        gettingKnockedBack = true;
        // gets the direction from the enemy to the player, normalizes it, and multiplies it by the knockback thrust and the mass of the rigidbody
        Vector2 knockbackDirection = (transform.position - damageSource.position).normalized;
        rigidBody.AddForce(knockbackDirection * knockbackThrust, ForceMode2D.Impulse);
        StartCoroutine(ResetKnockback());
    }

    public void GetKnockedBack(Vector2 direction, float knockbackThrust)
    {
        gettingKnockedBack = true;
        rigidBody.AddForce(direction * knockbackThrust, ForceMode2D.Impulse);
        StartCoroutine(ResetKnockback());
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackTime);
        gettingKnockedBack = false;
    }
}
