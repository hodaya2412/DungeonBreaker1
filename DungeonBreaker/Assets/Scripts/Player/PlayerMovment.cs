using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private bool isGrounded = true;

    private float directionX;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;

    [SerializeField] private PlayerHealth playerHealth; 

    private InputActions actionInput;

    private void Awake()
    {
        actionInput = new InputActions();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("RigidBody is null!");

        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("Animator is null!");

        sprite = GetComponent<SpriteRenderer>();
        if (sprite == null) Debug.LogError("SpriteRenderer is null!");
    }

    private void OnEnable()
    {
        actionInput.Player.Enable();
        actionInput.Player.Move.performed += OnMovePerformed;
        actionInput.Player.Move.canceled += OnMoveCanceled;
        actionInput.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        actionInput.Player.Move.performed -= OnMovePerformed;
        actionInput.Player.Move.canceled -= OnMoveCanceled;
        actionInput.Player.Jump.performed -= OnJumpPerformed;
        actionInput.Player.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        if (playerHealth != null && playerHealth.isDead) return;
        directionX = ctx.ReadValue<Vector2>().x;
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        if (playerHealth != null && playerHealth.isDead) return;
        directionX = 0;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (playerHealth != null && playerHealth.isDead) return;
        if (!isGrounded) return;
        Jump();
    }

    private void Update()
    {
        if (playerHealth != null && playerHealth.isDead) return;
        HandleMovmentAnimations();
    }

    private void FixedUpdate()
    {
        if (playerHealth != null && playerHealth.isDead) return;
        rb.linearVelocity = new Vector2(directionX * speed, rb.linearVelocity.y);
    }

    private void HandleMovmentAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        if (directionX != 0)
        {
            transform.localScale = new Vector3(
                directionX > 0 ? 1 : -1,
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void Jump()
    {
        isGrounded = false;
        animator.SetTrigger("IsJumping");
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
