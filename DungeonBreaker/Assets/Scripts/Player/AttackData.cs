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
    [Tooltip("האם השחקן יכול להפעיל מגן בשלב זה")]
    public bool hasShield = false;

    [Tooltip("אפקט של המגן שמופיע מעל השחקן בעת לחיצה על Q")]
    public GameObject shieldEffectPrefab;

    [Tooltip("כמה זמן המגן נשאר פעיל בשניות")]
    public float shieldDuration = 3f;

    [Tooltip("באיזה שלב המגן פעיל")]
    public int shieldActiveStage = 3;

}
