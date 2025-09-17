using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{ 
    [SerializeField] int maxHealth = 3;
    int currentHealth;

    private void OnEnable()
    {
        currentHealth = maxHealth;
        Events.OnPlayerAttack += TakeHit;
    }

    private void OnDisable()
    {
        Events.OnPlayerAttack -= TakeHit;
    }

    private void TakeHit(GameObject enemy, int damage)
    {
        if (enemy == gameObject)
        {
            currentHealth -= damage;

            EnemyMovement movement = GetComponent<EnemyMovement>();
            if (movement != null)
                movement.StopMoving();

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Events.OnEnemyDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }

}
