using System;
using UnityEngine;

public class Events
{
    public static Action<GameObject, int> OnPlayerAttack;
    public static Action<GameObject> OnEnemyDeath;
    public static Action<int, int> OnHealthChanged;
    public static Action<int> OnEnemyHitPlayer;

  
    public static Action OnAllEnemiesDefeated;
    public static Action OnGameOver;          
    public static Action<GameState> OnGameStateChanged;

    public static Action<GameObject, int> OnPlayerHeal;

    public static Action<EnemyMovement, IEnemyState> OnRequestStateChange;
    public static Action<EnemyMovement, bool> OnRequestMove;
    public static Action<EnemyMovement, int> OnRequestDirection;
    public static Action<EnemyMovement, bool> OnRequestAttackToggle;



    
    public static void RequestStateChange(EnemyMovement enemy, IEnemyState newState)
    {
        OnRequestStateChange?.Invoke(enemy, newState);
    }

    public static void RequestMove(EnemyMovement enemy, bool canMove)
    {
        OnRequestMove?.Invoke(enemy, canMove);
    }

    public static void RequestDirection(EnemyMovement enemy, int newDirection)
    {
        OnRequestDirection?.Invoke(enemy, newDirection);
    }
}
