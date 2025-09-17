using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyMovement enemyMovement; // ��� ����� �� ������ ���� ������
    [SerializeField] private int damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ����� �� �������� �� ������
            animator.SetTrigger("IsAttacking");

            // ���� �� ������ ���� ������
            if (enemyMovement != null)
                enemyMovement.StopMoving();

            // ����� ������� ���� ���
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }

    // ���� ������ ������� ������ ������ �������� ����� ������
    public void EndAttack()
    {
        if (enemyMovement != null)
            enemyMovement.ResumeMoving();
    }
}
