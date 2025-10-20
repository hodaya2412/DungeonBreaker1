using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenu; // גררי את PausePanel מהסצנה או השאירי ריק

    private bool isPaused = false;


    void Awake()
    {
        if (pauseMenu != null)
        {
            DontDestroyOnLoad(pauseMenu);

            // ודאי שהפאנל תמיד בראש
            Canvas canvas = pauseMenu.GetComponent<Canvas>();
            if (canvas != null)
                canvas.sortingOrder = 999; // מביא את הפאנל קדימה
        }
    }

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
        if (EventSystem.current == null)
        {
            Debug.LogWarning("אין EventSystem פעיל בסצנה!");
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
            pauseMenu.transform.SetAsLastSibling(); // מבטיח שהפאנל למעלה
        }

        isPaused = false;
        Time.timeScale = 1f;
    }


    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePause();
    }

    public void TogglePause()
    {
        if (pauseMenu == null)
        {
            Debug.LogWarning("PausePanel לא נמצא! ודאי שיש פאנל בשם PausePanel בסצנה");
            return;
        }

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

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
