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
    public GameObject attackEffectPrefab;       // האפקט שמופעל בזמן ההתקפה
    public GameObject powerUpGrantedEffect;     // אפקט שמופיע כשהשחקן מקבל את ההתקפה
}


