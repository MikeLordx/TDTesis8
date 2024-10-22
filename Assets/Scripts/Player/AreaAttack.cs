using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour
{
    public GameObject areaEffectPrefab;
    public float radius = 5f;
    public int damage = 50;
    public LayerMask groundLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CastAreaSpellFromCenter();
        }
    }

    void CastAreaSpellFromCenter()
    {
        // Define el centro de la pantalla
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Crea un rayo desde el centro de la cámara
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        // Realiza el Raycast y verifica si golpea el plano de terreno
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            // Instancia el área de efecto en el punto de impacto
            Instantiate(areaEffectPrefab, hit.point, Quaternion.identity);

            // Aplica daño en el área
            ApplyAreaDamage(hit.point);
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

}
