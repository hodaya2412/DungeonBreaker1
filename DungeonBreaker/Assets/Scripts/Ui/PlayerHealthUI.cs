using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    
    [Header("UI Elements")]
    [SerializeField] private Slider healthSlider;  
    [SerializeField] private Image fillImage;       

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 10;
    private const int MinHealth = 0;
    private int currentHealth;

    [Header("Appearance")]
    [SerializeField] private Color healthColor = Color.blue; 
    [SerializeField] private float animationSpeed = 2f;      

    private float targetValue;

    private void Awake()
    {
        currentHealth = maxHealth;
        targetValue = currentHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (fillImage != null)
            fillImage.color = healthColor;
    }

    private void Update()
    {
        
        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetValue, Time.deltaTime * animationSpeed);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, MinHealth);
        targetValue = currentHealth;
    }

    public void AddHealth(int amount)
    {
        targetValue += amount;
        targetValue = Mathf.Min(targetValue, healthSlider.maxValue);
    }

}
