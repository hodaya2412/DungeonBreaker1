using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] int maxHealth = 3;
    [SerializeField] Animator animator;
    [SerializeField] float hitStunTime = 0.3f; 

    int currentHealth;
    EnemyMovement movement;

    private void OnEnable()
    {
        currentHealth = maxHealth;
        Events.OnPlayerAttack += TakeHit;
        movement = GetComponent<EnemyMovement>();
    }
    private void OnDisable()
    {
        Events.OnPlayerAttack -= TakeHit;
    }

    private void TakeHit(GameObject enemy, int damage)
    {
        if (enemy == gameObject)
        {
            Debug.Log("TakeHit called!");
            currentHealth -= damage;

            if (animator != null)
                animator.SetTrigger("Hurt");

            if (movement != null)
                StartCoroutine(HitStun());

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private IEnumerator HitStun()
    {
        movement.StopMoving();
        yield return new WaitForSeconds(hitStunTime);
        movement.ResumeMoving();
    }

    private void Die()
    {
        if (animator != null)
            animator.SetTrigger("IsDead");

        Events.OnEnemyDeath?.Invoke(gameObject);

      
        StartCoroutine(DelayedDeath());
    }

    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(1f); 
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyKilled();
        }
        Destroy(gameObject);
    }

}
