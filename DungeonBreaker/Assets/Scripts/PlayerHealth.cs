using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    

    [SerializeField] int maxHealth = 5;
    int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        Events.OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Events.OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
    }
}