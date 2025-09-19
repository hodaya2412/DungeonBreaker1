using System;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static  Action<GameObject, int> OnPlayerAttack;
    public static  Action<GameObject> OnEnemyDeath;
    public static Action<int, int> OnHealthChanged;

    public static Action<GameObject, int> OnEnemyHitPlayer;
    
}
