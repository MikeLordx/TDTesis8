using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public GameObject projectilePrefab; // El prefab del proyectil
    public Transform firePoint; // El punto desde donde se lanzará el proyectil
    public float cooldownTime = 1f; // Tiempo de enfriamiento entre disparos

    private float nextFireTime = 0f;

    void Update()
    {
        // Si el jugador presiona la tecla "Q" y el ataque está listo
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > nextFireTime)
        {
            LaunchProjectile();
            nextFireTime = Time.time + cooldownTime; // Actualiza el tiempo de enfriamiento
        }
    }

    void LaunchProjectile()
    {
        // Instanciar el proyectil en el punto de lanzamiento
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}

