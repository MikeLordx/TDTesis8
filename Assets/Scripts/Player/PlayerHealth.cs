using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Cantidad m�xima de vida del jugador.")]
    public float maxHealth = 100f;

    [Tooltip("La barra de vida en la UI. D�jalo vac�o si no se usa.")]
    public Image healthBar;

    [Header("Respawn Settings")]
    [Tooltip("El tiempo en segundos antes de reaparecer.")]
    public float respawnTime = 10f;

    [Tooltip("El punto donde el jugador reaparecer�.")]
    public Transform respawnPoint;

    [Header("UI Elements")]
    [Tooltip("El texto para mostrar el contador.")]
    public TextMeshProUGUI countdownText;

    [SerializeField] private float currentHealth;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.fillAmount = maxHealth / 100;
            healthBar.fillAmount = currentHealth / 100;
        }

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.J))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / 100;
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
            healthBar.fillAmount = currentHealth / 100;
        }
        gameObject.SetActive(true);
        isDead = false;
        countdownText.gameObject.SetActive(false);
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / 100;
        }
    }
}