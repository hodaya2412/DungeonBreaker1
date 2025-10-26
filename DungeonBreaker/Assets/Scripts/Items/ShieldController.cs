using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ShieldController : MonoBehaviour
{
    [Header("Shield Settings from ScriptableObject")]
    [SerializeField] private AttackData attackData; 

    [Header("Player Stage")]
    [SerializeField] private int currentStage = 1;

    private const float DefaultShieldDuration = 10f;
    private InputActions inputActions;
    private bool shieldActive = false;
    private GameObject shieldInstance;

    private void Awake()
    {
        inputActions = new InputActions();

        
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
        
        if (shieldActive && shieldInstance != null)
        {
            shieldInstance.transform.position = transform.position;
        }
    }

    private void TryActivateShield()
    {
        if (attackData == null) return;

        
        if (!attackData.hasShield || currentStage != attackData.shieldActiveStage) return;

        ActivateShield();
    }

    public void ActivateShield()
    {
        if (shieldActive) return;

        shieldActive = true;
        Debug.Log(" Shield activated!");

        
        if (attackData.shieldEffectPrefab != null)
            shieldInstance = Instantiate(attackData.shieldEffectPrefab, transform.position, Quaternion.identity);

        
        StartCoroutine(ShieldDurationRoutine());
    }

    private IEnumerator ShieldDurationRoutine()
    {
        float timer = 0f;
        float duration = (attackData != null) ? attackData.shieldDuration : DefaultShieldDuration; 

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
        Debug.Log(" Shield deactivated!");

        
        if (shieldInstance != null)
            Destroy(shieldInstance);
    }

    public bool IsShieldActive() => shieldActive;
}
