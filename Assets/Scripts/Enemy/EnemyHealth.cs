using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int startingHealth = 100;

    [SerializeField]
    private float knockbackThrust = 15.0f;

    private int currentHealth;

    private Knockback knockback;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        // TODO
        Debug.Log("Health: " + currentHealth);

        knockback.GetKnockedBack(PlayerController.Instance.transform, knockbackThrust);

        DetectDeath();
    }

    private void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
