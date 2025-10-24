using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player & UI")]
    private PlayerAttack playerAttack;

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

        SetAttackDataByScene();

        // נבדוק אם זו הסצנה של התפריט הראשי
        if (scene.name == "StartScene") // ודאי ששם הסצנה תואם בדיוק ל-Build Settings
        {
            Events.OnGameStateChanged?.Invoke(GameState.MainMenu);
        }
        else
        {
            Events.OnGameStateChanged?.Invoke(GameState.Playing);
        }
    }


    private void SetAttackDataByScene()
    {
        if (playerAttack == null) return;
        string sceneName = SceneManager.GetActiveScene().name;
        // לוגיקה לפי שם סצנה אם תרצי
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

        Events.OnGameStateChanged?.Invoke(GameState.LevelComplete);
    }

    public void ShowGameOver()
    {
        Events.OnGameStateChanged?.Invoke(GameState.GameOver);
    }
}
