using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    LevelComplete,
    GameOver
}

public class GameStateManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public bool isMainMenuScene = false;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject pausePanel;
    public GameObject levelCompletePanel;
    public GameObject gameOverPanel;

    private GameState currentState;

    private void OnEnable()
    {
        Events.OnGameStateChanged += ChangeState;
    }

    private void OnDisable()
    {
        Events.OnGameStateChanged -= ChangeState;
    }

    private void Awake()
    {
        if (!isMainMenuScene && mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
    }

    private void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log($"[GameStateManager] State changed to {currentState}");

        mainMenuPanel?.SetActive(false);
        pausePanel?.SetActive(false);
        levelCompletePanel?.SetActive(false);
        gameOverPanel?.SetActive(false);

        switch (currentState)
        {
            case GameState.MainMenu:
                if (isMainMenuScene)
                {
                    mainMenuPanel?.SetActive(true);
                    Time.timeScale = 0f;
                }
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                break;

            case GameState.Paused:
                pausePanel?.SetActive(true);
                Time.timeScale = 0f;
                break;

            case GameState.LevelComplete:
                levelCompletePanel?.SetActive(true);
                break;

            case GameState.GameOver:
                gameOverPanel?.SetActive(true);
                Time.timeScale = 0f;
                break;
        }
    }

    public GameState GetState() => currentState;
}

