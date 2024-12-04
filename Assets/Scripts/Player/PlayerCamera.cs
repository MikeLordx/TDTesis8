using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.6f, 0);
    [SerializeField] private float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private Animator animator;
    private bool isAttacking = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        animator = playerBody.GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position = playerBody.position + offset;
        if (GameManager.instance.currentState == GameState.Playing)
        {
            HandleCameraRotation();
        }
    }

    private void HandleCameraRotation()
    {
        isAttacking = animator != null && animator.GetBool("IsAttacking");
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        if (!isAttacking)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
        transform.rotation = Quaternion.Euler(xRotation, playerBody.eulerAngles.y, 0f);
    }
}
