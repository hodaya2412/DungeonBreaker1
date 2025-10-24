using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonActions : MonoBehaviour
{
    [Header("Reference to GameStateManager")]
    [SerializeField] private GameStateManager gameStateManager;

    private void Start()
    {
        // אם לא חיברת את GameStateManager ידנית, ננסה למצוא אותו אוטומטית בסצנה
        if (gameStateManager == null)
        {
            gameStateManager = GameObject.FindObjectOfType<GameStateManager>();
            if (gameStateManager == null)
            {
                Debug.LogWarning("GameStateManager not found in scene!");
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        // נעדכן את מצב המשחק אחרי טעינת הסצנה
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void PauseGame()
    {
        gameStateManager?.ChangeState(GameState.Paused);
    }

    public void ResumeGame()
    {
        gameStateManager?.ChangeState(GameState.Playing);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void TryAgain()
    {
        Destroy(GameManager.Instance?.gameObject); // אם GameManager עדיין Singleton
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // נאתחל את GameStateManager מחדש בסצנה החדשה
        gameStateManager = GameObject.FindObjectOfType<GameStateManager>();
        if (gameStateManager != null)
        {
            gameStateManager.ChangeState(GameState.Playing);
        }
        else
        {
            Debug.LogWarning("GameStateManager not found after scene load.");
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
