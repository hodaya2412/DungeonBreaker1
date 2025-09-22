using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyDate enemyData;
    SpriteRenderer sprite;

    private EnemyMovement movement;
    private EnemyAttack attack;
    private EnemyHealth healthScript;

    void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyAttack>();
        healthScript = GetComponent<EnemyHealth>();
        LoadData();
    }

    void LoadData()
    {
        if (enemyData == null) { Debug.LogError("EnemyData not assigned!"); return; }

       
        if (movement != null) movement.speed = enemyData.speed;
        if (attack != null)
        {
            attack.damage = enemyData.damage;
            attack.detectionRange = enemyData.detectionRange;
        }
        if (healthScript != null) healthScript.currentHealth = enemyData.health;
    }
}
