using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyMovement enemyMovement; // כדי לעצור את התנועה בזמן התקיפה
    [SerializeField] private int damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // מפעיל את האנימציה של התקיפה
            animator.SetTrigger("IsAttacking");

            // עוצר את התנועה בזמן התקיפה
            if (enemyMovement != null)
                enemyMovement.StopMoving();

            // תגרום לשחקנית לקבל נזק
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }

    // אפשר להוסיף פונקציה שתופעל באירוע באנימציה לסיום התקיפה
    public void EndAttack()
    {
        if (enemyMovement != null)
            enemyMovement.ResumeMoving();
    }
}
