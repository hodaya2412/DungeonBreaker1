using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    private InputActions actionInput;
    private bool isPaused = false;

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

        
        if (isPaused)
            Events.OnGameStateChanged?.Invoke(GameState.Paused);
        else
            Events.OnGameStateChanged?.Invoke(GameState.Playing);
    }
}
