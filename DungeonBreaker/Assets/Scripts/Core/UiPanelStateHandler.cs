using UnityEngine;

public class UiPanelStateHandler : MonoBehaviour
{
    [SerializeField] private GameState relevantState;
    [SerializeField] private GameObject relevantPanel;

    private void OnEnable()
    {
        if (relevantPanel == null) relevantPanel = gameObject;
        Events.OnGameStateChanged += OnChangeState;
        if (GameStateManager.Instance != null)
            OnChangeState(GameStateManager.Instance.GetState()); 
    }

    private void OnDisable()
    {
        Events.OnGameStateChanged -= OnChangeState;
    }

    private void OnChangeState(GameState currentState)
    {
        relevantPanel.SetActive(currentState == relevantState);
    }
}
