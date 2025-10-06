using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyDate enemyData;
    [SerializeField] private int defaultDirection = 1;

    private float speed;
    private bool canMove = true;
    [SerializeField] private int direction = 1;
    private Rigidbody2D rb;

    [SerializeField] private IEnemyState currentState;

    public IEnemyState patrolState;
    public IEnemyState chaseState;
    public IEnemyState attackState;
    public IEnemyState idleState;
    public IEnemyState dieState;
    public IEnemyState spiritIdleState;

    private Transform player;

    private void OnEnable() => Events.OnEnemyDeath += OnAnyEnemyDeath;
    private void OnDisable() => Events.OnEnemyDeath -= OnAnyEnemyDeath;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (enemyData == null) enemyData = ScriptableObject.CreateInstance<EnemyDate>();
        speed = enemyData.speed;
        direction = (transform.localScale.x < 0 ? -1 : 1) * defaultDirection;
        player = FindPlayer();

        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);
        idleState = new IdleState(this);
        dieState = new DieState(this);
        spiritIdleState = new SpiritIdleState(this);

        // אתחול לפי סוג האויב
        if (enemyData.enemyType == EnemyType.Spirit)
        {
            ChangeState(spiritIdleState);
        }
        else
        {
            ChangeState(patrolState);
        }
    }



    void Update()
    {
        currentState?.Execute();

        if (canMove)
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        animator?.SetFloat("Speed", canMove ? Mathf.Abs(direction * speed) : 0);
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x = direction * defaultDirection;
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
        if (currentState == newState) return;
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public Animator GetAnimator() => animator;
    public float GetSpeed() => speed;
    public EnemyDate GetEnemyDate() => enemyData;
    public Transform GetPlayer() => player;

    public Transform FindPlayer()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p == null) p = GameObject.FindGameObjectWithTag("PlayerHitBox");
        return p != null ? p.transform : null;
    }

    public EnemyAttack GetEnemyAttack() => GetComponent<EnemyAttack>();

    private void OnAnyEnemyDeath(GameObject enemyObj)
    {
        if (enemyObj == this.gameObject)
        {
            ChangeState(dieState);
        }
    }
}
