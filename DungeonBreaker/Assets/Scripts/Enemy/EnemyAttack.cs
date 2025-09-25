using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyMovement enemyMovement;
    public int damage = 1;
    public float detectionRange = 5f;
    [SerializeField] private float attackCooldown = 2f;

    private Transform player;
    private float nextAttackTime = 0f;
    private int lastDirection = 1;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("PlayerHitBox");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null || enemyMovement == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange && Time.time >= nextAttackTime)
        {
            lastDirection = player.position.x > transform.position.x ? 1 : -1;
            enemyMovement.SetDirection(lastDirection);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHitBox") && Time.time >= nextAttackTime)
        {
            animator?.SetTrigger("IsAttacking");
            Events.OnEnemyHitPlayer?.Invoke(damage);

            nextAttackTime = Time.time + attackCooldown;
        }
    }
}