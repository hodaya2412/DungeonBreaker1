using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonActions : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void PauseGame()
    {
        Events.OnGameStateChanged?.Invoke(GameState.Paused);
    }

    public void ResumeGame()
    {
        Events.OnGameStateChanged?.Invoke(GameState.Playing);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void TryAgain()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Events.OnGameStateChanged?.Invoke(GameState.Playing);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
