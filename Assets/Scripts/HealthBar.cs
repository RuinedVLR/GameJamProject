using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using System;

public class HealthBar : MonoBehaviour
{
    [SerializeField] public Image healthBarSprite;
    
    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        healthBarSprite.fillAmount = currentHealth / maxHealth;
    }
}
