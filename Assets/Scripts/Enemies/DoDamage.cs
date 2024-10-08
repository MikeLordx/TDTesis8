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

        // Verifica si se encontr� el objeto con el script Player
        if (player == null)
        {
            Debug.LogError("No se encontr� ning�n objeto con el script Player en la escena.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player != null)
        {
            player.TakeDamage(10);
        }

        spawner.EnemyKilled();

        // Destruye el objeto actual despu�s de hacer da�o
        Destroy(gameObject);
    }
}

