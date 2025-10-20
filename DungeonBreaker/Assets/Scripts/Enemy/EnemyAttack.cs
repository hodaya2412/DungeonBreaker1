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

        // סובב את האויב לכיוון השחקן לפני ההתקפה
        if (player != null)
        {
            float direction = (player.position.x > transform.position.x) ? -1f : 1f;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * direction;
            transform.localScale = scale;
        }

        // אנימצית התקפה רגילה
        animator?.SetTrigger("IsAttacking");

        // נזק לשחקן
        Events.OnEnemyHitPlayer?.Invoke(damage);

        // אם זה בוס ויש התקפה מיוחדת
        if (enemyData.enemyType == EnemyType.Boss && enemyData.hasSpecialAttack
            && enemyData.specialAttackEffectPrefab != null)
        {
            Vector3 offset = new Vector3(5f * Mathf.Sign(transform.localScale.x), 0f, 0f);
            GameObject effect = Instantiate(
                enemyData.specialAttackEffectPrefab,
                transform.position + offset,
                Quaternion.identity
            );
            Destroy(effect, enemyData.specialAttackCooldown);
        }

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