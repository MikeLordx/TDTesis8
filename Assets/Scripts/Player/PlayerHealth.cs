using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Cantidad máxima de vida del jugador.")]
    public float maxHealth = 100f;

    [Tooltip("La barra de vida en la UI. Déjalo vacío si no se usa.")]
    public Slider healthBar;

    [Header("Respawn Settings")]
    [Tooltip("El tiempo en segundos antes de reaparecer.")]
    public float respawnTime = 10f;

    [Tooltip("El punto donde el jugador reaparecerá.")]
    public Transform respawnPoint;

    [Header("UI Elements")]
    [Tooltip("El texto para mostrar el contador.")]
    public TextMeshProUGUI countdownText;

    private float currentHealth;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        gameObject.SetActive(false);
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        countdownText.gameObject.SetActive(true);

        for (int i = (int)respawnTime; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        transform.position = respawnPoint.position;
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
        gameObject.SetActive(true);
        isDead = false;
        countdownText.gameObject.SetActive(false);

        Debug.Log("Player has respawned!");
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }
}