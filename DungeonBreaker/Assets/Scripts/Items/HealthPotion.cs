using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int healAmount = 1;
    public GameObject healEffectPrefab;
    private const float HealEffectLifetime = 3f;     

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            Events.OnPlayerHeal?.Invoke(other.gameObject, healAmount);

            
            if (healEffectPrefab != null)
            {
                GameObject effect = Instantiate(healEffectPrefab, other.transform.position, Quaternion.identity);
                effect.transform.SetParent(other.transform);
                Destroy(effect, HealEffectLifetime);
            }

            Destroy(gameObject);
        }
    }
}
