using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    // Singleton instance
    public static Player Instance { get; private set; }

    public int maxHealth = 100;               // Maximum health the player can have
    public int currentHealth;                 // Current health of the player
    public TMP_Text healthText;               // Reference to the TextMeshPro UI component

    public GameObject gameOverUI;

    private void Awake()
    {
        // Ensure that there's only one instance of Player
        if (Instance == null)
        {
            Instance = this;                  // Set the instance to this object
        }
        else if (Instance != this)
        {
            Destroy(gameObject);              // Destroy duplicate instances
            return;
        }

        // Ensure this object persists across scenes if necessary
        DontDestroyOnLoad(gameObject);         // Optional: if you want the player to persist across scenes
    }

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
        if (currentHealth <= 0) 
        {
            GameManager.instance.ChangeState(GameState.GameOver);
            Time.timeScale = 0f;
            gameOverUI.SetActive(true);
        }
    }

    // Function to update the displayed health value
    void UpdateHealthText()
    {
        healthText.text = "Health: " + currentHealth.ToString();  // Update the TMP text with the current health
    }
}
