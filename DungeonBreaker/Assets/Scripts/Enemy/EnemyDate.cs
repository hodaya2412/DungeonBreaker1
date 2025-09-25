using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDate", menuName = "Scriptable Objects/EnemyDate")]
public class EnemyDate : ScriptableObject
{
    public string enemyName;
    public float speed = 2f;
    public int health = 3;
    public int damage = 1;
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float hitStunTime = 0.3f;
    public float attackCooldown = 2f;
}
