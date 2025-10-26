using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private Animator animator;
    [SerializeField] private float deathAnimationDuration = 2f;
    [SerializeField] private PlayerHealthUI healthUI;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private ShieldController shieldController;

    private int currentHealth;
    public bool isDead = false;

    private void Awake()
    {
        ResetHealth();
    }

    private void OnEnable()
    {
        Events.OnEnemyHitPlayer += TakeDamage;
        Events.OnPlayerHeal += OnHeal; 
    }

    private void OnDisable()
    {
        Events.OnEnemyHitPlayer -= TakeDamage;
        Events.OnPlayerHeal -= OnHeal; 
    }

    private void OnHeal(GameObject player, int amount)
    {
        
        if (player == gameObject)
        {
            Heal(amount);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        if (shieldController != null && shieldController.IsShieldActive())
        {
            Debug.Log("Damage blocked by shield!");
            return;
        }

        currentHealth -= damage;
        Events.OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        healthUI?.TakeDamage(damage);

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
        GameManager.Instance?.ShowGameOver();
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Player healed! Current HP: " + currentHealth);
        Events.OnHealthChanged?.Invoke(currentHealth, maxHealth);
        healthUI?.AddHealth(amount);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        Events.OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
