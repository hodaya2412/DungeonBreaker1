using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float hitStunTime = 0.3f;
    [SerializeField] private EnemyHealthUI healthUI;
    [SerializeField] EnemyDate enemyData;

    private const float DeathAnimationDuration = 1f;

    private int currentHealth;
    private bool isDead = false;

    private void OnEnable()
    {
        currentHealth = enemyData.health;
        Events.OnPlayerAttack += TakeHit;
      
    }

    private void OnDisable()
    {
        Events.OnPlayerAttack -= TakeHit;
    }

    private void TakeHit(GameObject enemy, int damage)
    {
        Debug.Log($"💥 TakeHit called for {gameObject.name} (target: {enemy.name})");

        if (enemy != gameObject || isDead) return;

        Debug.Log("TakeHit called!");
        currentHealth -= damage;
        Events.OnEnemyHealthChanged?.Invoke(gameObject, currentHealth, enemyData.health);



        if (animator != null)
            animator.SetTrigger("Hurt");

        Events.OnRequestMove?.Invoke(GetComponent<EnemyMovement>(), false);


        if (currentHealth <= 0)
        {
            Die();
        }
        if (!isDead)
            StartCoroutine(HitStun());

    }

    private IEnumerator HitStun()
    {
        Events.OnRequestMove?.Invoke(GetComponent<EnemyMovement>(), false);
        yield return new WaitForSeconds(hitStunTime);
        Events.OnRequestMove?.Invoke(GetComponent<EnemyMovement>(), true);

    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Events.OnRequestMove?.Invoke(GetComponent<EnemyMovement>(), false);
        Events.OnRequestAttackToggle?.Invoke(GetComponent<EnemyMovement>(), false);




        Events.OnEnemyDeath?.Invoke(gameObject);

        StartCoroutine(DelayedDeath());
    }

    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(DeathAnimationDuration);
        Destroy(gameObject);
    }
}
