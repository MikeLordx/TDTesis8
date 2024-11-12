using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AreaAttack : MonoBehaviour
{
    public PlayerMana mana;
    public GameObject areaEffectPrefab;
    public float radius = 5f;
    public float cooldownTime = 1f;
    private float nextFireTime = 0f;
    public int damage = 50;
    public LayerMask groundLayer;

    // Nuevo: Coste de maná de la habilidad
    public float manaCost = 50f;

    // UI elements
    public Image cooldownImage;            // Imagen de la UI para mostrar el cooldown
    public TextMeshProUGUI cooldownText;   // Texto TMP para mostrar el tiempo restante del cooldown
    public Image lowManaImage;             // Imagen que se muestra si el maná es insuficiente

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Verifica si hay suficiente maná para el coste de la habilidad
            if (mana.currentMana < manaCost)
            {
                if (lowManaImage != null)
                {
                    StartCoroutine(BlinkLowManaImage());
                }
                return; // No se puede lanzar el ataque si el maná es insuficiente
            }

            // Si hay suficiente maná y el cooldown ha terminado, lanza el ataque
            if (Time.time > nextFireTime)
            {
                StartCoroutine(CastAreaSpellFromCenter());
                nextFireTime = Time.time + cooldownTime;

                if (cooldownImage != null && cooldownText != null)
                {
                    cooldownImage.gameObject.SetActive(true);
                    StartCoroutine(CooldownRoutine());
                }
            }
        }
    }

    IEnumerator CastAreaSpellFromCenter()
    {
        if (mana.currentMana < manaCost)
            yield break;

        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            GameObject vfx = Instantiate(areaEffectPrefab, hit.point, Quaternion.identity);
            mana.DecreaseMana(manaCost); // Resta el coste de maná especificado

            ApplyAreaDamage(hit.point);
            yield return new WaitForSeconds(4.3f);
            Destroy(vfx);
        }
    }

    void ApplyAreaDamage(Vector3 center)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (Collider hit in hitColliders)
        {
            TempEnemy enemy = hit.GetComponent<TempEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
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

        lowManaImage.gameObject.SetActive(false);
    }
}
