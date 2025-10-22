using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class BossSpecialAttack : MonoBehaviour
{
    [Header("Spawn settings")]
    [SerializeField] private GameObject[] enemiesToSpawn;

    [Header("Spawn area")]
    [SerializeField] private Transform spawnPointA;
    [SerializeField] private Transform spawnPointB;

    [Header("Timing")]
    [SerializeField] private float specialAttackCooldown = 120f;

    [Header("Boss references")]
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private string animatorTriggerName = "SpecialAttack";
    [SerializeField] private EnemyMovement bossMovement;

    private float nextSpecialAttackTime = 0f;
    private bool isRunning = false;

    private void Start()
    {
        nextSpecialAttackTime = Time.time + specialAttackCooldown;

        if (spawnPointA == null || spawnPointB == null)
            Debug.LogWarning($"[{name}] Spawn points not assigned.");
        if (enemiesToSpawn == null || enemiesToSpawn.Length == 0)
            Debug.LogWarning($"[{name}] No enemy prefabs assigned.");
    }

    private void Update()
    {
        if (isRunning) return;

        if (Time.time >= nextSpecialAttackTime)
        {
            StartCoroutine(SpecialAttackRoutine());
            nextSpecialAttackTime = Time.time + specialAttackCooldown;
        }
    }

    private IEnumerator SpecialAttackRoutine()
    {
        isRunning = true;

        // עצירת תנועה של הבוס (אם קיים)
        if (bossMovement != null)
            bossMovement.StopMoving();

        // הפעלת אנימציה של הבוס
        if (bossAnimator != null && !string.IsNullOrEmpty(animatorTriggerName))
            bossAnimator.SetTrigger(animatorTriggerName);

        // אפשר להוסיף כאן המתנה קצרה של כמה שניות לפני יצירת האויבים
        // yield return new WaitForSecondsRealtime(1f); // אם רוצים אנימציה קצרה

        // יצירת אויבים
        SpawnEnemies();

        // חזרה לתנועה של הבוס
        if (bossMovement != null)
            bossMovement.ResumeMoving();

        isRunning = false;
        yield return null;
    }

    private void SpawnEnemies()
    {
        if (enemiesToSpawn == null || enemiesToSpawn.Length == 0 || spawnPointA == null || spawnPointB == null)
            return;

        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            GameObject prefab = enemiesToSpawn[i];
            if (prefab == null) continue;

            float randomX = Random.Range(spawnPointA.position.x, spawnPointB.position.x);
            float randomY = Random.Range(spawnPointA.position.y, spawnPointB.position.y);
            Vector3 spawnPos = new Vector3(randomX, randomY, 0f);

            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }

    public void ForceSpecialAttackNow()
    {
        if (!isRunning)
        {
            nextSpecialAttackTime = Time.time + specialAttackCooldown;
            StartCoroutine(SpecialAttackRoutine());
        }
    }
}
