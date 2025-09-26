using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private string attackTrigger = "IsAttacking";

    [Header("UI & PowerUp")]
    [SerializeField] private GameObject powerUpPanel;

    private InputActions inputActions;
    private bool isAttacking = false;
    private AttackData currentAttackData;

    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }

    private void EnableInput()
    {
        inputActions.Player.Enable();
        inputActions.Player.Attack.performed += OnAttackPerformed;
    }

    private void DisableInput()
    {
        inputActions.Player.Attack.performed -= OnAttackPerformed;
        inputActions.Player.Disable();
    }

    public void ResetInput()
    {
        DisableInput();
        EnableInput();
    }

    public void InitializeAttack(AttackData newAttackData)
    {
        currentAttackData = newAttackData;
        ResetInput(); 
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (!isAttacking && currentAttackData != null)
        {
            animator?.SetTrigger(attackTrigger);
            StartCoroutine(DoAttack());
        }
    }

    private IEnumerator DoAttack()
    {
        isAttacking = true;
        attackCollider.enabled = true;

        if (currentAttackData.attackEffectPrefab != null)
        {
            GameObject effect = Instantiate(currentAttackData.attackEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, currentAttackData.attackDuration);
        }

        yield return new WaitForSeconds(currentAttackData.attackDuration);

        attackCollider.enabled = false;
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAttacking || currentAttackData == null) return;
        if (!other.CompareTag("Enemy")) return;

        Events.OnPlayerAttack?.Invoke(other.gameObject, currentAttackData.damage);
    }

    public void ActivatePowerUp()
    {
        if (currentAttackData != null && currentAttackData.powerUpGrantedEffect != null)
            Instantiate(currentAttackData.powerUpGrantedEffect, transform.position, Quaternion.identity);

        if (powerUpPanel != null)
            powerUpPanel.SetActive(true);
    }

    public void ShowPowerUpPanel()
    {
        if (powerUpPanel != null)
            powerUpPanel.SetActive(true);
    }
}
