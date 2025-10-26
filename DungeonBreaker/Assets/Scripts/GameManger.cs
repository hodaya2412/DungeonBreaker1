using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private float enemiesCheckDelay = 0.1f;
    private float initialTimeScale = 1f;
    public static GameManager Instance;

    [Header("Player & UI")]
    private PlayerAttack playerAttack;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Time.timeScale = initialTimeScale;
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

        
        if (scene.name == "StartScene") 
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
        
    }

    private void OnEnemyDeath(GameObject enemy)
    {
        StartCoroutine(CheckAllEnemiesDefeated());
    }

    private IEnumerator CheckAllEnemiesDefeated()
    {
        yield return new WaitForSeconds(enemiesCheckDelay);

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
