using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonActions : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "StartScene";
    [SerializeField] private string firstLevelSceneName = "Level1";

    public void StartGame()
    {
        SceneManager.sceneLoaded += OnSceneLoadedToPlay;
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void PauseGame() => GameStateManager.Instance.SetState(GameState.Paused);
    public void ResumeGame() => GameStateManager.Instance.SetState(GameState.Playing);

    public void TryAgain()
    {
        SceneManager.sceneLoaded += OnSceneLoadedToPlay;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextAndPlay()
    {
        SceneManager.sceneLoaded += OnSceneLoadedToPlay;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenu()
    {
        SceneManager.sceneLoaded += OnSceneLoadedToMenu;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame() => Application.Quit();

    private void OnSceneLoadedToPlay(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadedToPlay;
        GameStateManager.Instance.SetState(GameState.Playing, true);
    }

    private void OnSceneLoadedToMenu(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadedToMenu;
        GameStateManager.Instance.SetState(GameState.MainMenu, true);
    }
}
