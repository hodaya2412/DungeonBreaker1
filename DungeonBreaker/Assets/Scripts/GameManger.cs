using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerAttack playerAttack;
    private float enemiesCheckDelay = 0.1f;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        Events.OnEnemyDeath -= OnEnemyDeath;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            playerAttack = playerObj.GetComponent<PlayerAttack>();

        SetAttackDataByScene();

        
        if (scene.name == "StartScene")
            GameStateManager.Instance.SetState(GameState.MainMenu, true);
        else
            GameStateManager.Instance.SetState(GameState.Playing, true);
    }

    private void SetAttackDataByScene()
    {
        if (playerAttack == null) return;
        
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

       
        GameStateManager.Instance.SetState(GameState.LevelComplete);
    }

    public void ShowGameOver()
    {
        GameStateManager.Instance.SetState(GameState.GameOver);
    }
}
