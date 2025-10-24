using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int healAmount = 1;
    public GameObject healEffectPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 🎯 שולחים אירוע — השחקן שלקח שיקוי וזה כמות הריפוי
            Events.OnPlayerHeal?.Invoke(other.gameObject, healAmount);

            // אפקט ריפוי
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
