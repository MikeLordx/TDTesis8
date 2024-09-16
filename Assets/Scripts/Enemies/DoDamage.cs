using UnityEngine;

public class DoDamage : MonoBehaviour
{
    public Player player;

    private void OnTriggerEnter(Collider other)
    {
        player.TakeDamage(10);
        Destroy(gameObject);
    }
}
