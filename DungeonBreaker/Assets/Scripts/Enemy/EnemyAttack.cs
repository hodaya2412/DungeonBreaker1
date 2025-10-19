using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyDate enemyData;

    private int damage = 1;
    private float attackCooldown = 2f;
    private Transform player;
    private float nextAttackTime = 0f;

    private void Start()
    {
        if (enemyData != null)
        {
            damage = enemyData.damage;
            attackCooldown = enemyData.attackCooldown;
        }

        if (enemyMovement == null)
            enemyMovement = GetComponent<EnemyMovement>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("PlayerHitBox");
        if (playerObj == null)
            playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    public bool CanAttack()
    {
        return Time.time >= nextAttackTime;
    }

    public void TriggerAttack()
    {
        if (!CanAttack()) return;

        animator?.SetTrigger("IsAttacking");
        Events.OnEnemyHitPlayer?.Invoke(damage);
        nextAttackTime = Time.time + attackCooldown;
    }

    // זה יופעל אוטומטית כשהשחקנית נכנסת ל־Collider של האויב (טווח התקפה)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitBox") && CanAttack())
        {
            TriggerAttack();
        }
    }

    // אם רוצים להמשיך להפעיל התקפה גם כשהשחקנית נשארת בטווח:
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitBox") && CanAttack())
        {
            TriggerAttack();
        }
    }
}