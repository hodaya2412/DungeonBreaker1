using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] EnemyDate enemyData;
    float speed = 2f;
    bool canMove = true;

    private int direction = 1;
    Rigidbody2D rb;

    void Start()
    {
        speed = enemyData.speed;
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("RigidBody is null!");
        }
        direction = transform.localScale.x < 0 ? -1 : 1;
    }

    private void Update()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

            if (animator != null)
                animator.SetFloat("Speed", Mathf.Abs(direction * speed));
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            if (animator != null)
                animator.SetFloat("Speed", 0);
        }
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

    public void StopMoving()
    {
        canMove = false;
    }

    public void ResumeMoving()
    {
        canMove = true;
    }

    public void SetDirection(int newDirection)
    {
        if (newDirection != -1 && newDirection != 1)
            return;

        if (direction == newDirection)
            return;

        direction = newDirection;
        Flip();
    }
}