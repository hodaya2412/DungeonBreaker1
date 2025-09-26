using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] EnemyDate enemyData;

    private float speed;
    private bool canMove = true;
    private int direction = 1;
    private Rigidbody2D rb;

    private IEnemyState currentState;

   
    public IEnemyState patrolState;
    public IEnemyState chaseState;
    public IEnemyState attackState;
    public IEnemyState specialAttackState;
    public IEnemyState dieState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("RigidBody is null!");

        speed = enemyData.speed;
        direction = transform.localScale.x < 0 ? -1 : 1;

        
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);
        specialAttackState = new SpecialAttackState(this);
        dieState = new DieState(this);

        
        ChangeState(patrolState);
    }

    void Update()
    {
        currentState?.Execute();

        if (canMove)
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (animator != null)
            animator.SetFloat("Speed", canMove ? Mathf.Abs(direction * speed) : 0);
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * (direction == 1 ? 1 : -1);
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            direction *= -1;
            Flip();
        }
    }

    public void StopMoving() => canMove = false;
    public void ResumeMoving() => canMove = true;
    public void SetDirection(int newDirection)
    {
        if (newDirection != -1 && newDirection != 1) return;
        if (direction == newDirection) return;
        direction = newDirection;
        Flip();
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public Animator GetAnimator() => animator;
    public float GetSpeed() => speed;
}
