using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> wayPoint;
    public float playerDetectionRange = 5f;
    public float startSpacing = 2f;  // Espaciado inicial entre enemigos
    public float waypointAreaRadius = 3f; // Radio en el que los enemigos se moverán alrededor del waypoint

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private int currentWaypointIndex = 0;
    private Transform player;
    private PlayerHealth playerHealth;
    private bool chasingPlayer = false;
    private Vector3 targetPosition;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Colocar al enemigo en un área más organizada si es necesario
        Vector3 spawnPosition = wayPoint[currentWaypointIndex].position + new Vector3(Random.Range(-startSpacing, startSpacing), 0, Random.Range(-startSpacing, startSpacing));
        transform.position = spawnPosition;

        // Inicializa el destino alrededor del waypoint actual
        SetNewTargetPosition();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        if (player != null && playerHealth != null && !playerHealth.isDead)
        {
            if (Vector3.Distance(transform.position, player.position) <= playerDetectionRange)
            {
                chasingPlayer = true;
            }
            else
            {
                chasingPlayer = false;
            }

            if (chasingPlayer)
            {
                navMeshAgent.SetDestination(player.position);
            }
            else
            {
                // Continúa moviéndose hacia el waypoint sin detenerse
                if (Vector3.Distance(transform.position, targetPosition) <= 1f)
                {
                    SetNewTargetPosition(); // Cambiar a una nueva posición aleatoria cercana
                }
                navMeshAgent.SetDestination(targetPosition);
            }
        }
        else
        {
            chasingPlayer = false;
            // Mantener el movimiento hacia el waypoint sin detenerse
            if (Vector3.Distance(transform.position, targetPosition) <= 1f)
            {
                SetNewTargetPosition(); // Cambiar a una nueva posición aleatoria cercana
            }
            navMeshAgent.SetDestination(targetPosition);
        }

        if (navMeshAgent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    private void SetNewTargetPosition()
    {
        if (wayPoint == null || wayPoint.Count == 0)
        {
            return;
        }

        // Calcular un punto aleatorio dentro del área alrededor del waypoint
        Vector3 randomOffset = new Vector3(Random.Range(-waypointAreaRadius, waypointAreaRadius), 0, Random.Range(-waypointAreaRadius, waypointAreaRadius));
        targetPosition = wayPoint[currentWaypointIndex].position + randomOffset;

        // Cambiar al siguiente waypoint cuando el enemigo llega a la zona
        currentWaypointIndex = (currentWaypointIndex + 1) % wayPoint.Count;
    }
}
