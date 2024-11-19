using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterAttack : MonoBehaviour
{
    public GameObject projectilePrefab; // El prefab del proyectil
    public Transform firePoint;         // El punto desde donde se lanzará el proyectil
    public float cooldownTime = 1f;     // Tiempo de enfriamiento entre disparos

    public PlayerMana mana;             // Referencia al script de maná del jugador

    // Nuevo: Coste de maná para el ataque
    public float manaCost = 15f;

    private Animator animator;
    // UI elements
    public Image cooldownImage;          // Imagen de la UI para mostrar el cooldown
    public TextMeshProUGUI cooldownText; // Texto TMP para mostrar el tiempo restante del cooldown
    public Image lowManaImage;           // Imagen que se muestra si el maná es insuficiente
    public GameObject pause;

    private float nextFireTime = 0f;
    public bool pauseIsActive;

    private void Start()
    {
        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 0;
            cooldownImage.gameObject.SetActive(false);
        }

        if (cooldownText != null)
        {
            cooldownText.text = "";
            cooldownText.gameObject.SetActive(false);
        }

        if (lowManaImage != null)
        {
            lowManaImage.gameObject.SetActive(false);
        }
        animator = GetComponent<Animator>();
        pauseIsActive = false; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (mana.currentMana < manaCost)
            {
                if (lowManaImage != null)
                {
                    StartCoroutine(BlinkLowManaImage());
                }
                return;
            }

            if (Time.time > nextFireTime)
            {
                animator.SetBool("IsCasting", true);
                LaunchProjectile();
                nextFireTime = Time.time + cooldownTime;

                if (cooldownImage != null && cooldownText != null)
                {
                    cooldownImage.gameObject.SetActive(true);
                    StartCoroutine(CooldownRoutine());
                }
                StartCoroutine(ResetCasting());
            }
        }
    }
    IEnumerator ResetCasting()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsCasting", false);
    }

    void LaunchProjectile()
    {
        if (mana.currentMana < manaCost)
            return;

        // Resta el coste de maná
        mana.DecreaseMana(manaCost);

        // Instanciar el proyectil en el punto de lanzamiento
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }

    IEnumerator CooldownRoutine()
    {
        float cooldownTimer = cooldownTime;

        if (cooldownText != null)
        {
            cooldownText.gameObject.SetActive(true); // Activa el texto al inicio del cooldown
        }

        while (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownImage != null)
                cooldownImage.fillAmount = cooldownTimer / cooldownTime;

            if (cooldownText != null)
                cooldownText.text = Mathf.Ceil(cooldownTimer).ToString(); // Actualiza el texto con el tiempo restante

            yield return null;
        }

        if (cooldownImage != null)
            cooldownImage.gameObject.SetActive(false);

        if (cooldownText != null)
        {
            cooldownText.text = "";                 // Limpia el texto al finalizar
            cooldownText.gameObject.SetActive(false); // Desactiva el texto cuando termina el cooldown
        }
    }


    // Corrutina para parpadear la imagen de maná bajo durante 0.5 segundos
    IEnumerator BlinkLowManaImage()
    {
        float blinkDuration = 0.5f;
        float blinkInterval = 0.1f;
        float timer = 0f;

        while (timer < blinkDuration)
        {
            lowManaImage.gameObject.SetActive(!lowManaImage.gameObject.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // Asegurarse de que la imagen esté oculta al final
        lowManaImage.gameObject.SetActive(false);
    }
}
