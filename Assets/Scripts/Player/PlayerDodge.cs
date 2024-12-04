using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerDodge : MonoBehaviour
{
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private Transform cameraTransform;

    private Rigidbody rb;
    private bool isDashing = false;
    private float lastDashTime = -2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            Vector3 dashDirection = cameraTransform.forward;
            dashDirection.y = 0;

            StartCoroutine(PerformDash(dashDirection));
            lastDashTime = Time.time;
        }
    }

    IEnumerator PerformDash(Vector3 dashDirection)
    {
        isDashing = true;
        Vector3 startPosition = transform.position;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            rb.MovePosition(startPosition + dashDirection * dashDistance * (Time.time - startTime) / dashDuration);
            yield return null;
        }
        isDashing = false;
    }
}
