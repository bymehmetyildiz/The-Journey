using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    private Animator anim;
    public bool dead;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        dead = false;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetBool("isDead", true);
        Destroy(gameObject, 2);
        dead = true;

    }

}
