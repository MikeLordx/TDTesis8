using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    [Header("Mana Settings")]
    [Tooltip("Cantidad máxima de maná del jugador.")]
    public float maxMana = 100f;

    [Tooltip("La barra de maná en la UI. Déjalo vacío si no se usa.")]
    public Image manaBar;

    public float currentMana;

    [Tooltip("Cantidad de maná que se regenera por segundo.")]
    public float manaRegenRate = 5f;

    [Tooltip("Tiempo de espera entre cada regeneración de maná (en segundos).")]
    public float regenInterval = 1f;

    [Tooltip("Tiempo de espera antes de comenzar la regeneración (en segundos).")]
    public float regenStartDelay = 1.5f;

    private Coroutine regenCoroutine;

    private void Start()
    {
        currentMana = maxMana;
        UpdateManaBar();
    }

    public void DecreaseMana(float decrease)
    {
        // Detener la corrutina de regeneración si ya está en ejecución
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }

        // Reducir el maná y actualizar la barra de maná
        currentMana -= decrease;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        UpdateManaBar();

        // Si el maná es menor al máximo, iniciar la regeneración
        if (currentMana < maxMana)
        {
            regenCoroutine = StartCoroutine(RegenerateMana());
        }
    }

    private IEnumerator RegenerateMana()
    {
        // Espera el tiempo de retraso antes de comenzar la regeneración
        yield return new WaitForSeconds(regenStartDelay);

        // Mientras el maná esté por debajo del máximo
        while (currentMana < maxMana)
        {
            // Aumentar el maná según la tasa de regeneración
            currentMana += manaRegenRate;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana); // Limitar el valor máximo de maná
            UpdateManaBar();

            // Esperar el intervalo de regeneración antes de continuar
            yield return new WaitForSeconds(regenInterval);
        }

        // Cuando el maná llega al máximo, detener la corrutina
        regenCoroutine = null;
    }

    private void UpdateManaBar()
    {
        if (manaBar != null)
        {
            manaBar.fillAmount = currentMana / maxMana;
        }
    }
}
