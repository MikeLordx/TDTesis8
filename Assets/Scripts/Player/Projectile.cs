using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float maxLifetime = 5f; // El tiempo que vive el proyectil si no golpea nada
    public int damage = 50;

    private float lifeTimer;

    void Start()
    {
        lifeTimer = maxLifetime;
    }

    void Update()
    {
        // Mover el proyectil hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Contador de vida del proyectil
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject); // Destruye el proyectil después del tiempo máximo
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Verificar si el proyectil golpea a un enemigo
        if (other.CompareTag("Enemy"))
        {
            TempEnemy enemy = other.GetComponent<TempEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Aplica daño al enemigo
            }

            Destroy(gameObject); // Destruye el proyectil tras golpear al enemigo
        }
    }
}
