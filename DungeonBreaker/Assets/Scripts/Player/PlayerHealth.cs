using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private Animator animator;

    private int currentHealth;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        Events.OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void OnEnable()
    {
        Events.OnEnemyHitPlayer += TakeHitFromEnemy;
    }

    private void OnDisable()
    {
        Events.OnEnemyHitPlayer -= TakeHitFromEnemy;
    }

    private void TakeHitFromEnemy(GameObject player, int damage)
    {
        if (player == gameObject)
        {
            TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Events.OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator?.SetTrigger("IsHit");
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

     
        animator?.SetTrigger("IsDead");


      
        StartCoroutine(StopGameAfterDeath(7));
    }


    private System.Collections.IEnumerator StopGameAfterDeath(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 0f; 
    }
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Player healed! Current HP: " + currentHealth);
    }
}
