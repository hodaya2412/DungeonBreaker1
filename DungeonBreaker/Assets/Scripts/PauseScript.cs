using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [Header("Pause Panel")]
    public GameObject pauseMenu; // גררי את הפאנל כאן ב-Inspector

    private bool isPaused = false;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // תמיד סגור את הפאנל בסצנה חדשה
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        isPaused = false;
        Time.timeScale = 1f;
    }

    public void TogglePause()
    {
        if (pauseMenu == null) return;

        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        if (EventSystem.current != null)
            EventSystem.current.sendNavigationEvents = !isPaused;
    }

    public void Resume()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        isPaused = false;
        Time.timeScale = 1f;

        if (EventSystem.current != null)
            EventSystem.current.sendNavigationEvents = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMenu(string menuSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
