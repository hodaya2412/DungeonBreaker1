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

    private float menuTimeScale = 0f;       
    private float playingTimeScale = 1f;    
    private float pausedTimeScale = 0f;     
    private float gameOverTimeScale = 0f;   
    


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
                    Time.timeScale = menuTimeScale;
                }
                break;

            case GameState.Playing:
                Time.timeScale = playingTimeScale;
                break;

            case GameState.Paused:
                pausePanel?.SetActive(true);
                Time.timeScale = pausedTimeScale;
                break;

            case GameState.LevelComplete:
                levelCompletePanel?.SetActive(true);
                break;

            case GameState.GameOver:
                gameOverPanel?.SetActive(true);
                Time.timeScale = gameOverTimeScale;
                break;
        }
    }

    public GameState GetState() => currentState;
}

