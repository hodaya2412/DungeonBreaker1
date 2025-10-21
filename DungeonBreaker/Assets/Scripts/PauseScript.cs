using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;

    void Awake()
    {
        if (pauseMenu != null)
        {
            DontDestroyOnLoad(pauseMenu);
            Canvas canvas = pauseMenu.GetComponent<Canvas>();
            if (canvas != null)
                canvas.sortingOrder = 999;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;


    }
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (EventSystem.current == null)
            Debug.LogWarning("[PauseScript] אין EventSystem פעיל בסצנה!");

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
            pauseMenu.transform.SetAsLastSibling();
        }

        isPaused = false;
        Time.timeScale = 1f;
    }



    public void TogglePause()
    {
        if (pauseMenu == null)
        {
            Debug.LogWarning("[PauseScript] PausePanel לא נמצא!");
            return;
        }

        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        if (EventSystem.current != null)
            EventSystem.current.sendNavigationEvents = !isPaused;

        Debug.Log($"[PauseScript] TogglePause: {isPaused}");
    }

    public void Resume()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        isPaused = false;
        Time.timeScale = 1f;

        if (EventSystem.current != null)
            EventSystem.current.sendNavigationEvents = true;

        Debug.Log("[PauseScript] Resume");
    }

    public void QuitGame()
    {
        Debug.Log("[PauseScript] QuitGame");
        Application.Quit();
    }
}
