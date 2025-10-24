using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player & UI")]
    private PlayerAttack playerAttack;

    [Header("Managers")]
    [SerializeField] private GameStateManager gameStateManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Time.timeScale = 1f;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            playerAttack = playerObj.GetComponent<PlayerAttack>();

        // נאתחל את GameStateManager מחדש אם הוא לא מחובר
        if (gameStateManager == null)
        {
            GameObject managerObj = GameObject.Find("GameStateManager");
            if (managerObj != null)
                gameStateManager = managerObj.GetComponent<GameStateManager>();
        }

        SetAttackDataByScene();
    }

    private void SetAttackDataByScene()
    {
        if (playerAttack == null) return;

        string sceneName = SceneManager.GetActiveScene().name;
        // כאן אפשר להוסיף לוגיקה לפי שם הסצנה
    }

    private void OnEnemyDeath(GameObject enemy)
    {
        StartCoroutine(CheckAllEnemiesDefeated());
    }

    private IEnumerator CheckAllEnemiesDefeated()
    {
        yield return new WaitForSeconds(0.1f);

        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            yield return null;

        Debug.Log("[GameManager] All enemies defeated!");
        Events.OnAllEnemiesDefeated?.Invoke();

        if (playerAttack != null)
        {
            playerAttack.ShowPowerUpPanel();
            playerAttack.ActivatePowerUp();
        }

        gameStateManager?.ChangeState(GameState.LevelComplete);
    }

    public void ShowGameOver()
    {
        gameStateManager?.ChangeState(GameState.GameOver);
    }
}
