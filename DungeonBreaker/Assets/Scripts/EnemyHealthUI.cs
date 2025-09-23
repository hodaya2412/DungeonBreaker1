using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;   
    [SerializeField] private Image fillImage;       
    [SerializeField] private int maxHealth = 3;     
    [SerializeField] private Color healthColor = Color.green; 
    [SerializeField] private float animationSpeed = 2f; 

    private float targetValue; 
    private int currentHealth;

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
        currentHealth = Mathf.Max(currentHealth, 0);
        targetValue = currentHealth; 
    }
 

}
