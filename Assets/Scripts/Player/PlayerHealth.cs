using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public Image healthBar;
    [SerializeField] public float respawnTime = 10f;
    [SerializeField] public Transform respawnPoint;
    [SerializeField] public TextMeshProUGUI countdownText;
    [SerializeField] private float currentHealth;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        /*if (Input.GetKeyUp(KeyCode.J))
        {
            TakeDamage(10);
        }*/
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
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
        UpdateHealthUI();
        gameObject.SetActive(true);
        isDead = false;
        countdownText.gameObject.SetActive(false);
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }
}