using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player & UI")]
    [SerializeField] private PlayerAttack playerAttack;   
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
    }

    private void OnDisable()
    {
        Events.OnEnemyDeath -= OnEnemyDeath;
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
            playerAttack.ActivatePowerUp();
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
