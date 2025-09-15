using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] Collider2D attackCollider; 
    [SerializeField] float attackDuration = 0.3f; 

    InputActions inputActions;
    bool isAttacking = false;

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
            StartCoroutine(DoAttack());
        }
    }

    private System.Collections.IEnumerator DoAttack()
    {
        isAttacking = true;
        attackCollider.enabled = true; 

        yield return new WaitForSeconds(attackDuration);

        attackCollider.enabled = false;
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttacking && other.CompareTag("Enemy"))
        {
            
            Events.OnPlayerAttack?.Invoke(other.gameObject, damage);
        }
    }
}
