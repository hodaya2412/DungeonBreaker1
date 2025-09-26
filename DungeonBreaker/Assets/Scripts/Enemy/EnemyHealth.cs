using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float hitStunTime = 0.3f;
    [SerializeField] private EnemyHealthUI healthUI;
    [SerializeField] EnemyDate enemyData;

    private int currentHealth;
    private EnemyMovement movement;
    private EnemyAttack attack;
    private bool isDead = false;

    private void OnEnable()
    {
        currentHealth = enemyData.health;
        Events.OnPlayerAttack += TakeHit;
        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyAttack>();
    }

    private void OnDisable()
    {
        Events.OnPlayerAttack -= TakeHit;
    }

    private void TakeHit(GameObject enemy, int damage)
    {
        if (enemy != gameObject || isDead) return;

        Debug.Log("TakeHit called!");
        currentHealth -= damage;

        if (healthUI != null)
            healthUI.TakeDamage(damage);

        if (animator != null)
            animator.SetTrigger("Hurt");

        if (movement != null)
            StartCoroutine(HitStun());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitStun()
    {
        if (movement != null)
            movement.StopMoving();
        yield return new WaitForSeconds(hitStunTime);
        if (!isDead && movement != null)
            movement.ResumeMoving();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // עצירת תנועה והתקפה
        if (movement != null)
            movement.StopMoving();
        if (attack != null)
            attack.enabled = false; // מונע התקפות נוספות

        if (animator != null)
            animator.SetTrigger("IsDead");

        Events.OnEnemyDeath?.Invoke(gameObject);

        StartCoroutine(DelayedDeath());
    }

    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
