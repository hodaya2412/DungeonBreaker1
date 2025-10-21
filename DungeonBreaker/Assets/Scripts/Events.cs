using System;
using UnityEngine;

public class Events
{
    public static Action<GameObject, int> OnPlayerAttack;
    public static Action<GameObject> OnEnemyDeath;
    public static Action<int, int> OnHealthChanged;
    public static Action<int> OnEnemyHitPlayer;

    public static Action<GameState> OnStateEnter;
    public static Action<GameState> OnStateExit;
    public static Func<GameState> OnGetCurrentState;

    // 🆕 חדשים:
    public static Action OnAllEnemiesDefeated;
    public static Action OnGameOver;          // ✅ כשהשחקן מת
    public static Action OnRetryPressed;

}
