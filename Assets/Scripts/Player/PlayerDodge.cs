using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerDodge : MonoBehaviour
{
    [SerializeField] private float dodgeDistance = 5f;
    [SerializeField] private float dodgeDuration = 0.2f;
    [SerializeField] private float dodgeCooldown = 2f;
    [SerializeField] private Transform cameraTransform;

    private Rigidbody rb;
    private bool isDodging = false;
    private float lastDodgeTime = -2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodging && Time.time >= lastDodgeTime + dodgeCooldown)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            if (move != Vector3.zero)
            {
                Vector3 dodgeDirection = cameraTransform.TransformDirection(move).normalized;
                StartCoroutine(PerformDodge(dodgeDirection));
                lastDodgeTime = Time.time;
            }
        }
    }

    IEnumerator PerformDodge(Vector3 dodgeDirection)
    {
        isDodging = true;
        float startTime = Time.time;

        while (Time.time < startTime + dodgeDuration)
        {
            rb.velocity = new Vector3(dodgeDirection.x * (dodgeDistance / dodgeDuration), rb.velocity.y, dodgeDirection.z * (dodgeDistance / dodgeDuration));
            yield return null;
        }

        isDodging = false;
    }
}