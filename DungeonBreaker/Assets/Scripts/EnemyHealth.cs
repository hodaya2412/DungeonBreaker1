using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] Animator animator; // הוספת Animator
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

            // עוצר תנועה בזמן התקיפה/פגיעה
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
        // מפעיל את האנימציה של המוות
        if (animator != null)
            animator.SetTrigger("IsDead");

        // קריאה לאירוע מוות אם צריך
        Events.OnEnemyDeath?.Invoke(gameObject);

        // השמדת האויב אחרי השהייה כדי לאפשר לאנימציה לרוץ
        Destroy(gameObject, 1f); // 1 שניה או לפי אורך האנימציה
    }
}
