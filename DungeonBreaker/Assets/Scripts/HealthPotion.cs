using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int healAmount = 1;
    public GameObject healEffectPrefab; // Prefab של האפקט

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
            }

            
            if (healEffectPrefab != null)
            {
               
                GameObject effect = Instantiate(healEffectPrefab, other.transform.position, Quaternion.identity);
                effect.transform.SetParent(other.transform);
                Destroy(effect, 3f);
            }

            Destroy(gameObject); 
        }
    }
}
