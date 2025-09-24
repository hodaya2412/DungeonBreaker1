using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private Animator animator;
    [SerializeField] private float deathAnimationDuration = 2f;
    [SerializeField] private PlayerHealthUI healthUI;
    [SerializeField] private GameObject gameOverPanel;

    private int currentHealth;
    public bool isDead = false;

    private void Awake()
    {
        ResetHealth(); 
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

        if (healthUI != null)
            healthUI.TakeDamage(damage);

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

        StartCoroutine(StopGameAfterDeath(deathAnimationDuration));
    }

    private IEnumerator StopGameAfterDeath(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Player healed! Current HP: " + currentHealth);
        Events.OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (healthUI != null)
            healthUI.AddHealth(amount);
    }

   
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        Events.OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
