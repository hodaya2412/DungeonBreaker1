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
    public static GameStateManager Instance;

    private GameState currentState;
    private const float TIME_STOPPED = 0f;
    private const float TIME_NORMAL = 1f;


    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentState = GameState.MainMenu;
        Time.timeScale = TIME_STOPPED; 
    }

    private void OnEnable()
    {
        Events.OnPlayerDied += OnPlayerDied;
        Events.OnLevelCompleted += OnLevelCompleted;
    }

    private void OnDisable()
    {
        Events.OnPlayerDied -= OnPlayerDied;
        Events.OnLevelCompleted -= OnLevelCompleted;
    }

    public void SetState(GameState newState, bool forceInvoke = false)
    {
        if (!forceInvoke && newState == currentState) return;

        currentState = newState;
        Debug.Log($"[GameStateManager] -> {newState}");


        Time.timeScale = (newState == GameState.Playing || newState == GameState.LevelComplete)? TIME_NORMAL: TIME_STOPPED;



        Events.OnGameStateChanged?.Invoke(newState);
    }

    public GameState GetState() => currentState;

    private void OnPlayerDied() => SetState(GameState.GameOver, true);
    private void OnLevelCompleted() => SetState(GameState.LevelComplete, true);
}
