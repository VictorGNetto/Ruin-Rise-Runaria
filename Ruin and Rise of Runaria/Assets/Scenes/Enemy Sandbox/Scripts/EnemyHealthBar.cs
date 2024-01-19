using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Color healthLow;
    public Color healthHight;
    public Vector3 healthOffset;

    public void SetHealth(float health, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

        healthSlider.fillRect.GetComponent<Image>().color = Color.Lerp(healthLow, healthHight, healthSlider.normalizedValue);
    }

    void Update()
    {
        healthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + healthOffset);
    }
}
