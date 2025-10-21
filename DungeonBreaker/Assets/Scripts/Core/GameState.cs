using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public StateSO stateData;
    public bool isCurrentState;
    public GameState previousState;
    GameState nextState;
    List<TransitionBase> transitions = new();
    public bool wasTransitionedInto;
    public bool inTransition;

    // ✅ משתנה למעבר אוטומטי אחרי שהאויבים מתו
    private bool allEnemiesDefeated = false;
    [SerializeField] private GameState nextStateAfterEnemiesDefeated;

    void Start()
    {
        transitions.AddRange(GetComponentsInChildren<TransitionBase>());
        isCurrentState = Events.OnGetCurrentState?.Invoke() == this;

        // ✅ מאזין לאירוע “כל האויבים מתו”
        Events.OnAllEnemiesDefeated += OnAllEnemiesDefeated;

        Debug.Log($"[GameState] {name} initialized. Is current: {isCurrentState}");
    }

    private void OnDestroy()
    {
        Events.OnAllEnemiesDefeated -= OnAllEnemiesDefeated;
    }

    private void OnAllEnemiesDefeated()
    {
        allEnemiesDefeated = true;
        Debug.Log($"[GameState] AllEnemiesDefeated event received in state {name}");
    }

    void Update()
    {
        if (!isCurrentState) { return; }

        nextState = null;

        // ✅ בדיקה למעבר אוטומטי
        if (allEnemiesDefeated && nextStateAfterEnemiesDefeated != null)
        {
            nextState = nextStateAfterEnemiesDefeated;
            allEnemiesDefeated = false;
            Debug.Log($"[GameState] Transition triggered from {name} to {nextState.name} (all enemies defeated)");
        }

        // בדיקה ל־Transitions רגילים
        foreach (var transition in transitions.Where(x => x.ShouldTransition()))
        {
            if (transition.TargetState != null)
            {
                nextState = transition.TargetState;
                Debug.Log($"[GameState] Transition triggered from {name} to {nextState.name}");
                break;
            }
        }

        if (nextState != null && !inTransition)
        {
            inTransition = true;
            Debug.Log($"[GameState] Exiting state: {name}");
            StateExit();
            inTransition = false;
        }

        if (wasTransitionedInto)
        {
            Debug.Log($"[GameState] State {name} was transitioned into");
            wasTransitionedInto = false;
        }
    }

    private void StateExit()
    {
        isCurrentState = false;
        Events.OnStateExit?.Invoke(this);

        Debug.Log($"[GameState] Exited {name}. Entering next state: {nextState.name}");
        nextState.StateEnter(this);
    }

    private void StateEnter(GameState previousState)
    {
        wasTransitionedInto = true;
        this.previousState = previousState;
        isCurrentState = true;

        Debug.Log($"[GameState] Entered state: {name} (previous: {(previousState != null ? previousState.name : "None")})");
        Events.OnStateEnter?.Invoke(this);
    }
}
