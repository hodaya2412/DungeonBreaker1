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

    private void OnEnable()
    {
        Events.OnEnemyDeath += OnEnemyDeath;
    }

    private void OnDisable()
    {
        Events.OnEnemyDeath -= OnEnemyDeath;
    }

    private void Start()
    {
        // סופרים את כל האויבים בתחילת הסצנה
        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("Enemies in scene: " + enemiesRemaining);
    }

    private void OnEnemyDeath(GameObject enemy)
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
        // כאן אפשר להפעיל מעבר לשלב הבא, לתת פרס או הפעלה אחרת
    }
}
