using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;               // Maximum health the player can have
    public int currentHealth;                 // Current health of the player
    public TMP_Text healthText;               // Reference to the TextMeshPro UI component

    void Start()
    {
        currentHealth = maxHealth;            // Set the initial health to max health at the start
        UpdateHealthText();                   // Update the health display
    }

    // Call this function whenever the player's health changes (e.g., when taking damage)
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;              // Reduce the player's current health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't drop below 0 or exceed maxHealth
        UpdateHealthText();                   // Update the health display
    }

    // Function to update the displayed health value
    void UpdateHealthText()
    {
        healthText.text = "Health: " + currentHealth.ToString();  // Update the TMP text with the current health
    }
}
