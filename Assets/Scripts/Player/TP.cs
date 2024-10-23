using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP : MonoBehaviour
{
    [SerializeField] public Transform connectedTeleport;
    public float cooldownTime = 20f;

    private bool canTeleport = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canTeleport && connectedTeleport != null)
        {
            other.transform.position = connectedTeleport.position;
            StartCoroutine(TeleportCooldown());
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
        if (connectedTeleport != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, connectedTeleport.position);
        }
    }
}
