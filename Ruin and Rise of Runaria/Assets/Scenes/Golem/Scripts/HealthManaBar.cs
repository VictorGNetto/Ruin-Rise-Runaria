using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManaBar : MonoBehaviour
{
    public Slider healthSlider;
    public Color healthLow;
    public Color healthHight;
    public Vector3 healthOffset;

    public Slider manaSlider;
    public Color manaLow;
    public Color manaHight;
    public Vector3 manaOffset;

    public void SetHealth(float health, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

        healthSlider.fillRect.GetComponent<Image>().color = Color.Lerp(healthLow, healthHight, healthSlider.normalizedValue);
    }

    public void SetMana(float mana, float maxMana)
    {
        manaSlider.maxValue = maxMana;
        manaSlider.value = mana;

        manaSlider.fillRect.GetComponent<Image>().color = Color.Lerp(manaLow, manaHight, manaSlider.normalizedValue);
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + healthOffset);
        manaSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + manaOffset);
    }
}
