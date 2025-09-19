using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private int damage = 1;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackCooldown = 2f; // זמן בין התקפות

    private Transform player;
    private bool hasAttacked = false;
    private float nextAttackTime = 0f;
    private int lastDirection = 1; // זוכר את הכיוון האחרון

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null || enemyMovement == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange && !hasAttacked && Time.time >= nextAttackTime)
        {
            lastDirection = player.position.x > transform.position.x ? 1 : -1;
            enemyMovement.SetDirection(lastDirection);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasAttacked && Time.time >= nextAttackTime)
        {
            // התקפה
            animator?.SetTrigger("IsAttacking");
            Events.OnEnemyHitPlayer?.Invoke(collision.gameObject, damage);

            hasAttacked = true;
            nextAttackTime = Time.time + attackCooldown;

            enemyMovement?.StopMoving();
            Invoke(nameof(TurnAroundAfterAttack), 0.5f); // זמן לסיום האנימציה
        }
    }

    private void TurnAroundAfterAttack()
    {
        // הופך כיוון
        lastDirection *= -1;
        enemyMovement?.SetDirection(lastDirection);
        enemyMovement?.ResumeMoving();

        StartCoroutine(WaitForPlayerToLeave());
    }

    private System.Collections.IEnumerator WaitForPlayerToLeave()
    {
        while (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            yield return null;
        }

        hasAttacked = false;
    }
}
