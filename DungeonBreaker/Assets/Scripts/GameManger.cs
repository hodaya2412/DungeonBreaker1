using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player & UI")]
    private PlayerAttack playerAttack; // מחפש אוטומטית את השחקן בסצנה
    [SerializeField] private GameObject gameOverPanel;

    [Header("Stage Attack ScriptableObjects")]
    [SerializeField] private AttackData stage1Attack;
    [SerializeField] private AttackData stage2Attack;

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

        if (sceneName == "Level1" && stage1Attack != null)
            playerAttack.InitializeAttack(stage1Attack);
        else if (sceneName == "Level2" && stage2Attack != null)
            playerAttack.InitializeAttack(stage2Attack);
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
