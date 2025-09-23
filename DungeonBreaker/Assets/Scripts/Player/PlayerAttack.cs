
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro; 

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int normalDamage = 1;
    [SerializeField] private int poweredDamage = 3;
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private float attackDuration = 0.3f;
    [SerializeField] private Animator animator;
    [SerializeField] private string attackTrigger = "IsAttacking";

    [Header("Visual Effects")]
    [SerializeField] private GameObject powerUpGrantedEffect; 
    [SerializeField] private GameObject poweredAttackEffect; 

    [Header("UI")]
    [SerializeField] private TMP_Text powerUpText;

    private InputActions inputActions;
    private bool isAttacking = false;
    private bool poweredUp = false;

    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Attack.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttackPerformed;
        inputActions.Player.Disable();
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (!isAttacking)
        {
            animator?.SetTrigger(attackTrigger);
            StartCoroutine(DoAttack());
        }
    }

    private IEnumerator DoAttack()
    {
        isAttacking = true;
        attackCollider.enabled = true;

        yield return new WaitForSeconds(attackDuration);

        attackCollider.enabled = false;
        isAttacking = false;

        if (poweredUp && poweredAttackEffect != null)
        {
            Instantiate(poweredAttackEffect, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttacking && other.CompareTag("Enemy"))
        {
            int damageToDeal = poweredUp ? poweredDamage : normalDamage;
            Events.OnPlayerAttack?.Invoke(other.gameObject, damageToDeal);
        }
    }

    
    public void ActivatePowerUp()
    {
        Debug.Log("ActivatePowerUp נקראה!");
        Debug.Log("PowerUpText קיים? " + (powerUpText != null));
        if (poweredUp) return; 
        poweredUp = true;

        Debug.Log("Power-up activated! Damage increased to " + poweredDamage);

        
        if (powerUpGrantedEffect != null)
        {
            Instantiate(powerUpGrantedEffect, transform.position, Quaternion.identity);
        }

        
        if (powerUpText != null)
        {
            StartCoroutine(ShowPowerUpUI());
        }
    }

   
    private IEnumerator ShowPowerUpUI()
    {
        powerUpText.gameObject.SetActive(true);           
        yield return new WaitForSeconds(2f);              
        powerUpText.gameObject.SetActive(false);          
    }
}
