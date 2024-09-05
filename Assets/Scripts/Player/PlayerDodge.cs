using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    [SerializeField] public CharacterController controller;
    [SerializeField] public float dodgeDistance = 5f;
    [SerializeField] public float dodgeDuration = 0.2f;
    [SerializeField] public float dodgeCooldown = 2f;

    [SerializeField] private bool isDodging = false;
    [SerializeField] private float lastDodgeTime = -2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodging && Time.time >= lastDodgeTime + dodgeCooldown)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            if (move != Vector3.zero)
            {
                StartCoroutine(PerformDodge(move));
                lastDodgeTime = Time.time;
            }
        }
    }

    IEnumerator PerformDodge(Vector3 moveDirection)
    {
        isDodging = true;
        Vector3 dodgeDirection = transform.right * moveDirection.x + transform.forward * moveDirection.z;
        float startTime = Time.time;

        while (Time.time < startTime + dodgeDuration)
        {
            controller.Move(dodgeDirection * (dodgeDistance / dodgeDuration) * Time.deltaTime);
            yield return null;
        }

        isDodging = false;
    }
}