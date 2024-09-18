using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] public CharacterController controller;
    [SerializeField] public float jumpHeight = 3f;
    [SerializeField] public float gravity = -9.81f;

    [SerializeField] private Vector3 velocity;
    [SerializeField] private bool isGrounded;

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        Vector3 move = new Vector3(0, velocity.y, 0);
        controller.Move(move * Time.deltaTime);
    }
}