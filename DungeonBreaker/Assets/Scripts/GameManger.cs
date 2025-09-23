// GameManager.cs
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private PlayerAttack playerAttack;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Events.OnEnemyDeath += OnEnemyDeath;
    }

    private void OnDisable()
    {
        Events.OnEnemyDeath -= OnEnemyDeath;
    }

    private void OnEnemyDeath(GameObject enemy)
    {
        // קורוטינה שתבדוק מתי אין יותר אויבים עם תג "Enemy"
        StartCoroutine(CheckAllEnemiesDefeated());
    }

    private IEnumerator CheckAllEnemiesDefeated()
    {
        // חכה מעט כדי שהאויבים יסיימו את כל תהליכי המוות שלהם
        yield return new WaitForSeconds(0.1f);

        // בודק שוב אם באמת אין אויבים עם תג "Enemy"
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            yield return null;
        }

        // עכשיו בטוח שכל האויבים מתו, הפעל את ה-Power-Up
        if (playerAttack != null)
            playerAttack.ActivatePowerUp();
    }
}
