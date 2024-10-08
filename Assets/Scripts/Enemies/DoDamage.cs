using UnityEngine;

public class DoDamage : MonoBehaviour
{
    private Player player;
    private WaveSpawner spawner;

    private void Start()
    {
        // Busca el componente Player en la escena y lo asigna
        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<WaveSpawner>();

        // Verifica si se encontró el objeto con el script Player
        if (player == null)
        {
            Debug.LogError("No se encontró ningún objeto con el script Player en la escena.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player != null)
        {
            player.TakeDamage(10);
        }

        spawner.EnemyKilled();

        // Destruye el objeto actual después de hacer daño
        Destroy(gameObject);
    }
}

