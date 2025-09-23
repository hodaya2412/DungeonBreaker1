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
        // �������� ������ ��� ��� ���� ������ �� �� "Enemy"
        StartCoroutine(CheckAllEnemiesDefeated());
    }

    private IEnumerator CheckAllEnemiesDefeated()
    {
        // ��� ��� ��� �������� ������ �� �� ������ ����� ����
        yield return new WaitForSeconds(0.1f);

        // ���� ��� �� ���� ��� ������ �� �� "Enemy"
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            yield return null;
        }

        // ����� ���� ��� ������� ���, ���� �� �-Power-Up
        if (playerAttack != null)
            playerAttack.ActivatePowerUp();
    }
}
