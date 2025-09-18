using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;


public class PlayerMovment : MonoBehaviour

{

    [SerializeField] float jumpForce;
    [SerializeField] float speed;
    [SerializeField] bool isGrounded = true;
    float directionX;
    private Rigidbody2D rb;
    InputActions actionInput;
    Animator animator;
    SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("RigidBody is null!");
        }
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator is null!");
        }
        animator.enabled = false;
        animator.Update(0);
        sprite = GetComponent<SpriteRenderer>();
        if (sprite == null)
        {
            Debug.LogError("sprite is null!");
        }
        StartCoroutine(EnableAnimator());

    }
    IEnumerator EnableAnimator()
    {
        yield return new WaitForSeconds(1f);
        animator.enabled = true;
    }
    private void Awake()
    {
        actionInput = new InputActions();

    }
    private void OnEnable()
    {
        actionInput.Player.Enable();
        actionInput.Player.Move.performed += OnMovePerformed;
        actionInput.Player.Move.canceled += OnMoveCanceled;
        actionInput.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (!isGrounded) return;
        Jump();
    }

    private void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        directionX = 0;
    }

    private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        directionX = ctx.ReadValue<Vector2>().x;

    }
    private void OnDisable()
    {
        actionInput.Player.Disable();
        actionInput.Player.Move.performed -= OnMovePerformed;
        actionInput.Player.Move.canceled -= OnMoveCanceled;
        actionInput.Player.Jump.performed -= OnJumpPerformed;
    }

    // Update is called once per frame
    void Update()
    {

        HandleMovmentAnimations();
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
    private void FixedUpdate()
    {
        rb.linearVelocity = new(directionX * speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("toched ground");
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
