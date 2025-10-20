using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameState currentState;

    [Header("Player & UI")]
    private PlayerAttack playerAttack;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Stage Attack ScriptableObjects")]
    [SerializeField] private AttackData stage1Attack;
    [SerializeField] private AttackData stage2Attack;
    [SerializeField] private AttackData stage3Attack;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Time.timeScale = 1f;

            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Events.OnEnemyDeath += OnEnemyDeath;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        Events.OnEnemyDeath -= OnEnemyDeath;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        // קריאה בטוחה לפונקציות של GameState
        currentState?.Update();
    }

    public void ChangeState(GameState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    // --- כאשר סצנה נטענת ---
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            playerAttack = playerObj.GetComponent<PlayerAttack>();

        SetAttackDataByScene();
    }

    // --- קביעה של נתוני ההתקפה לפי שלב ---
    private void SetAttackDataByScene()
    {
        if (playerAttack == null) return;

        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Level1":
                if (stage1Attack != null) playerAttack.InitializeAttack(stage1Attack);
                break;
            case "Level2":
                if (stage2Attack != null) playerAttack.InitializeAttack(stage2Attack);
                break;
            case "Level3":
                if (stage3Attack != null) playerAttack.InitializeAttack(stage3Attack);
                break;
        }
    }

    // --- כשאויב מת ---
    private void OnEnemyDeath(GameObject enemy)
    {
        StartCoroutine(CheckAllEnemiesDefeated());
    }

    private IEnumerator CheckAllEnemiesDefeated()
    {
        yield return new WaitForSeconds(0.1f);

        // לחכות עד שכל האויבים הושמדו
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            yield return null;

        if (playerAttack != null)
        {
            playerAttack.ShowPowerUpPanel();
            playerAttack.ActivatePowerUp();
        }
    }

    // --- מסך סיום משחק ---
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("GameManager: GameOverPanel לא משויך ב-Inspector!");
        }
    }

    // --- מעבר לשלב הבא ---
    public void LoadNextLevel()
    {
        string current = SceneManager.GetActiveScene().name;

        switch (current)
        {
            case "Level1":
                SceneManager.LoadScene("Level2");
                break;
            case "Level2":
                SceneManager.LoadScene("Level3");
                break;
            default:
                // חזרה לתפריט בסוף המשחק
                SceneManager.LoadScene("StartScreen");
                break;
        }
    }

    // --- חזרה לתפריט הראשי ---
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScreen");
    }

    // --- התחלה מחדש של השלב ---
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }
}
