using System.Collections;
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
    public bool isDead = false;
    [SerializeField] private Camera respawnCamera;
    [SerializeField] private Camera playerCamera;
    public AudioClip hit;
    public AudioClip playerDie;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
        if (respawnCamera != null)
        {
            respawnCamera.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        AudioManager.instance.PlaySFX(hit);

        if (currentHealth <= 0)
        {
            Die();
            AudioManager.instance.PlaySFX(playerDie);
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
        var collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;

        var movement = GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;

        if (respawnCamera != null)
        {
            respawnCamera.gameObject.SetActive(true);
            Debug.Log("Respawn camera activated.");
        }
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(false);
            Debug.Log("Player camera deactivated.");
        }

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }
        for (int i = (int)respawnTime; i > 0; i--)
        {
            if (countdownText != null)
            {
                countdownText.text = i.ToString();
            }
            yield return new WaitForSeconds(1f);
        }
        transform.position = respawnPoint.position + Vector3.up * 0.5f;
        currentHealth = maxHealth;
        UpdateHealthUI();
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
        }
        UnityEngine.AI.NavMeshAgent navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;
        }

        Collider playerCollider = GetComponent<Collider>();
        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        if (respawnCamera != null)
        {
            respawnCamera.gameObject.SetActive(false);
        }
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
        isDead = false;
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }
}