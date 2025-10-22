using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ShieldController : MonoBehaviour
{
    [Header("Shield Settings from ScriptableObject")]
    [SerializeField] private AttackData attackData; // ScriptableObject שמכיל hasShield, shieldEffectPrefab, shieldDuration, shieldActiveStage

    [Header("Player Stage")]
    [SerializeField] private int currentStage = 1; // נעדכן לפי מצב המשחק

    private InputActions inputActions;
    private bool shieldActive = false;
    private GameObject shieldInstance;

    private void Awake()
    {
        inputActions = new InputActions();

        // חיבור הכפתור Q להפעלת המגן
        inputActions.Player.Shield.started += ctx => TryActivateShield();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        // אם יש מגן פעיל, נדאג שהוא יעקוב אחרי השחקנית
        if (shieldActive && shieldInstance != null)
        {
            shieldInstance.transform.position = transform.position;
        }
    }

    private void TryActivateShield()
    {
        if (attackData == null) return;

        // בדיקה אם המגן פעיל בסקריפטבל אובייקט ואם בשלב הנכון
        if (!attackData.hasShield || currentStage != attackData.shieldActiveStage) return;

        ActivateShield();
    }

    public void ActivateShield()
    {
        if (shieldActive) return;

        shieldActive = true;
        Debug.Log("🛡 Shield activated!");

        // יצירת אפקט המגן
        if (attackData.shieldEffectPrefab != null)
            shieldInstance = Instantiate(attackData.shieldEffectPrefab, transform.position, Quaternion.identity);

        // התחלת זמן המגן
        StartCoroutine(ShieldDurationRoutine());
    }

    private IEnumerator ShieldDurationRoutine()
    {
        float timer = 0f;
        float duration = (attackData != null) ? attackData.shieldDuration : 10f; // ברירת מחדל 10 שניות

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        DeactivateShield();
    }

    public void DeactivateShield()
    {
        if (!shieldActive) return;

        shieldActive = false;
        Debug.Log("🛡 Shield deactivated!");

        // השמדת האפקט
        if (shieldInstance != null)
            Destroy(shieldInstance);
    }

    public bool IsShieldActive() => shieldActive;
}
