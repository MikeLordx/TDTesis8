using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP : MonoBehaviour
{
    [Header("Teleport Configuration")]
    public Transform connectedTeleport; // El TP al que se conectar� este punto.
    public float cooldownTime = 5f; // Tiempo de espera antes de que se pueda usar de nuevo.

    private bool canTeleport = true;

    private void OnTriggerStay(Collider other)
    {
        // Verifica si el objeto que entra tiene el tag "Player" y si el TP est� listo para ser usado.
        if (other.CompareTag("Player") && canTeleport && connectedTeleport != null)
        {
            // Teletransporta al jugador a la posici�n del TP conectado.
            other.transform.position = connectedTeleport.position;

            // Inicia el cooldown para ambos puntos de TP.
            StartCoroutine(TeleportCooldown());

            // Tambi�n inicia el cooldown en el TP conectado.
            TP connectedTeleportScript = connectedTeleport.GetComponent<TP>();
            if (connectedTeleportScript != null)
            {
                connectedTeleportScript.StartCoroutine(connectedTeleportScript.TeleportCooldown());
            }
        }
    }

    private IEnumerator TeleportCooldown()
    {
        canTeleport = false;
        yield return new WaitForSeconds(cooldownTime);
        canTeleport = true;
    }

    private void OnDrawGizmos()
    {
        // Dibuja una l�nea en el editor para visualizar la conexi�n entre TPs.
        if (connectedTeleport != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, connectedTeleport.position);
        }
    }
}
