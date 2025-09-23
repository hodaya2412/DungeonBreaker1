using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;   // Slider של המד חיים
    [SerializeField] private Image fillImage;       // Fill של ה-Slider
    [SerializeField] private int maxHealth = 3;     // חיים מקסימליים
    [SerializeField] private Color healthColor = Color.green; // צבע קבוע
    [SerializeField] private float animationSpeed = 2f; // כמה מהר המד חיים יורד

    private float targetValue; // הערך שה-Slider אמור להגיע אליו
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
        // אנימציה פשוטה: הערך מתקרב בהדרגה ל-targetValue
        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetValue, Time.deltaTime * animationSpeed);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        targetValue = currentHealth; // הערך החדש שהאנימציה תגיע אליו
    }
 

}
