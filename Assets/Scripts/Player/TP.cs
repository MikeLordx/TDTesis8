using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP : MonoBehaviour
{
    [SerializeField] public Transform connectedTeleport;
    public float cooldownTime = 20f;

    private bool canTeleport = true;
    private Renderer objectRenderer;
    private Color originalColor;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCharacter") && canTeleport && connectedTeleport != null)
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
        if (objectRenderer != null)
        {
            objectRenderer.material.color = Color.red;
        }
        yield return new WaitForSeconds(cooldownTime);
        canTeleport = true;
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
        }
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
