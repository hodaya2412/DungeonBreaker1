using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player & UI")]
    private PlayerAttack playerAttack;
    [SerializeField] private GameObject gameOverPanel;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);

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
        yield return new WaitForSeconds(0.1f);

        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            yield return null;

        Debug.Log("[GameManager] All enemies defeated!");
        Events.OnAllEnemiesDefeated?.Invoke(); // ✅ זה מה שגורם למכונת המצבים לדעת

        if (playerAttack != null)
        {
            playerAttack.ShowPowerUpPanel();
            playerAttack.ActivatePowerUp();
        }
    }



    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            Time.timeScale = 0f;
            gameOverPanel.SetActive(true);
        }
    }
}