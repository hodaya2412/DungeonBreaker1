// PlayerAttack.cs
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro; // חשוב עבור TextMeshPro

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
    [SerializeField] private GameObject powerUpGrantedEffect; // מופעל פעם אחת אחרי סיום כל האויבים
    [SerializeField] private GameObject poweredAttackEffect;  // מופעל בזמן התקפות לאחר הפאוור-אפ

    [Header("UI")]
    [SerializeField] private TMP_Text powerUpText; // TextMeshPro שמודיע על Power-Up

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

        // אפקט התקפה משודרג מופעל רק אחרי הפאוור-אפ
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

    // מופעל על ידי GameManager אחרי שכל האויבים מתו באמת
    public void ActivatePowerUp()
    {
        Debug.Log("ActivatePowerUp נקראה!");
        Debug.Log("PowerUpText קיים? " + (powerUpText != null));
        if (poweredUp) return; // מוודא שזה קורה פעם אחת בלבד
        poweredUp = true;

        Debug.Log("Power-up activated! Damage increased to " + poweredDamage);

        // אפקט חד-פעמי מיד אחרי סיום כל האויבים
        if (powerUpGrantedEffect != null)
        {
            Instantiate(powerUpGrantedEffect, transform.position, Quaternion.identity);
        }

        // הפעלת UI שמודיע על Power-Up
        if (powerUpText != null)
        {
            StartCoroutine(ShowPowerUpUI());
        }
    }

    // קורוטינה להצגת הטקסט ל-2 שניות
    private IEnumerator ShowPowerUpUI()
    {
        powerUpText.gameObject.SetActive(true);           // מציג את הטקסט
        yield return new WaitForSeconds(2f);              // מחכה 2 שניות
        powerUpText.gameObject.SetActive(false);          // מסתיר שוב
    }
}
