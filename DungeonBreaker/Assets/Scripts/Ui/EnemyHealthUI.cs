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

    
    private void OnEnable()
    {
        Events.OnEnemyHealthChanged += UpdateHealthBar;
    }

   
    private void OnDisable()
    {
        Events.OnEnemyHealthChanged -= UpdateHealthBar;
    }

    private void Update()
    {
        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetValue, Time.deltaTime * animationSpeed);
        }
    }

    
    private void UpdateHealthBar(GameObject owner, int current, int max)
    {
        
        if (owner != gameObject)
            return;

        currentHealth = current;
        targetValue = current;
        maxHealth = max;

        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
        }
    }
}
