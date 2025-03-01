using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;  // Required to work with UI components like Slider

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBar;  // Reference to the health bar (Slider)

    void Start()
    {
        // Initialize health values
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;  // Set max value of health bar
            healthBar.value = currentHealth; // Set current health value
        }
        else
        {
            UnityEngine.Debug.LogError("HealthBar reference is missing in the Inspector!");
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            Die();  // Call the death method when health reaches zero
        }

        if (healthBar != null)
        {
            healthBar.value = currentHealth;  // Update health bar value
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;  // Ensure health doesn't exceed max value
        }

        if (healthBar != null)
        {
            healthBar.value = currentHealth;  // Update health bar value
        }
    }

    // Optional: Handle player death
    void Die()
    {
        gameObject.SetActive(false);  // Disable the player object
        UnityEngine.Debug.Log("Player has died!");
        // Add logic for what happens when the player dies (e.g., respawn or end game)
    }
}
