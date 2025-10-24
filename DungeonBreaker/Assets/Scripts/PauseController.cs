using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel; // גררי את הפאנל של הפאוז לכאן
    private bool isPaused = false;
    private InputActions actionInput;

    private void Awake()
    {
        actionInput = new InputActions();
    }

    private void OnEnable()
    {
        actionInput.Player.Enable();
        actionInput.Player.Menu.performed += OnPausePerformed;
    }

    private void OnDisable()
    {
        actionInput.Player.Menu.performed -= OnPausePerformed;
        actionInput.Player.Disable();
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }
}
