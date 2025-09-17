using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyMovement enemyMovement; 
    [SerializeField] private int damage = 1;

    private bool isAttacking = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            isAttacking = true;

            animator.SetTrigger("IsAttacking");

            if (enemyMovement != null)
                enemyMovement.StopMoving();

            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isAttacking = false;

            if (enemyMovement != null)
                enemyMovement.ResumeMoving();
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
        if (enemyMovement != null)
            enemyMovement.ResumeMoving();
    }
}
