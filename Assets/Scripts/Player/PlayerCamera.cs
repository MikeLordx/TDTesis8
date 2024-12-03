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
        animator = playerBody.GetComponent<Animator>();
    }

    private void Update()
    {
        // Siempre sigue la posición del jugador
        transform.position = playerBody.position + offset;

        // Solo rota si el GameState es Playing
        if (GameManager.instance.currentState == GameState.Playing)
        {
            HandleCameraRotation();
        }
    }

    private void HandleCameraRotation()
    {
        // Detectar si el jugador está atacando
        isAttacking = animator != null && animator.GetBool("IsAttacking");

        // Movimiento del ratón
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotación vertical de la cámara
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -20f, 20f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotación horizontal del jugador (solo si no está atacando)
        if (!isAttacking)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
