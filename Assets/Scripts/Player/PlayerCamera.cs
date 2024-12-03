using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.6f, 0);
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float smoothTime = 0.1f;

    private float xRotation = 0f;
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;

    private Animator animator;
    private bool isAttacking = false;

    private void Start()
    {
        animator = playerBody.GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.instance.currentState == GameState.Playing)
        {
            isAttacking = animator != null && animator.GetBool("IsAttacking");
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -20f, 20f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            if (!isAttacking)
            {
                playerBody.Rotate(Vector3.up * mouseX);
            }
            Quaternion targetRotation = Quaternion.Euler(0f, playerBody.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, 0f);
            transform.position = playerBody.position + offset;
        }
    }

}
