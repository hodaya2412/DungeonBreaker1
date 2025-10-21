using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    List<GameState> states = new();
    [SerializeField] GameState currentState;
    [SerializeField] GameState defaultState;

    void Awake()
    {
        states.AddRange(GetComponentsInChildren<GameState>());

        Events.OnStateEnter += StateEnter;
        Events.OnGetCurrentState += GetCurrentState;
        SceneManager.sceneLoaded += AnnounceStateOnSceneLoad;

        if (currentState == null)
            currentState = defaultState;
        Events.OnStateEnter?.Invoke(currentState);
    }

    private void AnnounceStateOnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        if (currentState == null)
            Events.OnStateEnter?.Invoke(defaultState);
        else
            Events.OnStateEnter?.Invoke(currentState);
    }

    private GameState GetCurrentState()
    {
        return currentState;
    }

    private void StateEnter(GameState state)
    {
        currentState = state;
    }
}
