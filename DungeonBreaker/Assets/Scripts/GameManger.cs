using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int enemiesRemaining;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("Enemies in scene: " + enemiesRemaining);
    }

    
    public void EnemyKilled()
    {
        enemiesRemaining--;
        Debug.Log("Enemy killed! Remaining: " + enemiesRemaining);

        if (enemiesRemaining <= 0)
        {
            AllEnemiesDefeated();
        }
    }

    private void AllEnemiesDefeated()
    {
        Debug.Log("All enemies defeated! Grant power-up here.");
        // כאן אפשר להפעיל את השדרוג/כוח חדש לשחקנית
    }
}
