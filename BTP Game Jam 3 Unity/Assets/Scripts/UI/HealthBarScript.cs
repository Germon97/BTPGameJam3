using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField]
    private Image[] healthImages;
    [SerializeField]
    private Sprite fullHeartSprite;
    [SerializeField]
    private Sprite emptyHeartSprite;
    [SerializeField]
    private PlayerHealthScript playerHealth;

    private int maxHealth;
    private int currentHealth;

    private void OnEnable()
    {
        playerHealth.ChangedHealth.AddListener(ChangeHealth);
        playerHealth.ChangedMaxHealth.AddListener(ChangeMaxHealth);
    }

    private void OnDisable()
    {
        playerHealth.ChangedHealth.RemoveListener(ChangeHealth);
        playerHealth.ChangedMaxHealth.RemoveListener(ChangeMaxHealth);
    }

    public void ChangeHealth(int health)
    {
        currentHealth = health;
        UpdateUI();
    }

    public void ChangeMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < healthImages.Length; i++)
        {
            healthImages[i].enabled = i < maxHealth;
        }

        for (int i = 0; i < maxHealth; i++)
        {
            healthImages[i].sprite = (i < currentHealth) ? fullHeartSprite : emptyHeartSprite;
        }
    }
}
