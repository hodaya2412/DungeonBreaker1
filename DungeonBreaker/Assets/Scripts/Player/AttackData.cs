using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackData : ScriptableObject
{
    [Header("Basic Info")]
    public string attackName;

    [Header("Attack Stats")]
    public int damage;
    public float attackDuration = 0.3f;

    [Header("Visual Effects")]
    public GameObject attackEffectPrefab;       
    public GameObject powerUpGrantedEffect;     

    [Header("Shield Settings")]
    public bool hasShield = false;
    public GameObject shieldEffectPrefab;
    public float shieldDuration = 3f; 
    public int shieldActiveStage = 3;

}
