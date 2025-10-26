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
    private float spawnDepth = 0f;

    [Header("Timing")]
    [SerializeField] private float specialAttackCooldown = 120f;

    [Header("Boss references")]
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private string animatorTriggerName = "SpecialAttack";
    [SerializeField] private EnemyMovement bossMovement;

    [Header("VFX")]
    [SerializeField] private GameObject specialAttackEffectPrefab;               
    [SerializeField, Min(0f)] private float effectLifetimeSec = 2f;  
    

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

        
        if (bossMovement != null)
            bossMovement.StopMoving();

  
        if (bossAnimator != null && !string.IsNullOrEmpty(animatorTriggerName))
            bossAnimator.SetTrigger(animatorTriggerName);

        PlaySpecialAttackVFX();

        SpawnEnemies();

        if (bossMovement != null)
            bossMovement.ResumeMoving();

        isRunning = false;
        yield return null;
    }

    private void PlaySpecialAttackVFX()
    {
        if (specialAttackEffectPrefab == null) return;

        GameObject fx = Instantiate(specialAttackEffectPrefab, transform.position, Quaternion.identity);

        if (effectLifetimeSec > 0f)
            Destroy(fx, effectLifetimeSec);
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
            Vector3 spawnPos = new Vector3(randomX, randomY, spawnDepth);

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
