using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    private InputActions input;
    private bool isPaused = false;

    private void Awake()
    {
        input = new InputActions();
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Menu.performed += OnPause;
    }

    private void OnDisable()
    {
        input.Player.Menu.performed -= OnPause;
        input.Player.Disable();
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        isPaused = !isPaused;

        if (isPaused)
            GameStateManager.Instance.SetState(GameState.Paused);
        else
            GameStateManager.Instance.SetState(GameState.Playing);
    }
}