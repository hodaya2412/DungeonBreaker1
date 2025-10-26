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
    private float effectLifetimeSec = 2f;

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
        nextAttackTime = Time.time + attackCooldown;
        Events.OnEnemyHitPlayer?.Invoke(damage);

        if (enemyData != null && enemyData.enemyType == EnemyType.Boss)
        {
            if (enemyData.specialAttackEffectPrefab != null && player != null)
            {

                GameObject effect = Instantiate(
                    enemyData.specialAttackEffectPrefab,
                    player.position,
                    Quaternion.identity
                );


                Destroy(effect, effectLifetimeSec);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitBox") && CanAttack())
        {
            TriggerAttack();
        }
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHitBox") && CanAttack())
        {
            TriggerAttack();
        }
    }
}