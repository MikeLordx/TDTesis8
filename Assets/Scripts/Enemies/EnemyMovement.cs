using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> wayPoint;
    public float playerDetectionRange = 5f;
    public float startSpacing = 2f;  // Espaciado inicial entre enemigos
    public float waypointAreaRadius = 1f; // Radio en el que los enemigos se moverán alrededor del waypoint

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

    private void Update()
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
                HandleWaypointMovement();
            }
        }
        else
        {
            chasingPlayer = false;
            HandleWaypointMovement();
        }

        UpdateAnimator();
    }

    private void HandleWaypointMovement()
    {
        // Moverse hacia el waypoint objetivo
        if (Vector3.Distance(transform.position, targetPosition) <= 1f)
        {
            AdvanceToNextWaypoint();
        }
        navMeshAgent.SetDestination(targetPosition);
    }

    private void AdvanceToNextWaypoint()
    {
        // Asegúrate de avanzar al siguiente waypoint solo si hay waypoints definidos
        if (wayPoint == null || wayPoint.Count == 0) return;

        // Cambiar al siguiente waypoint
        currentWaypointIndex = (currentWaypointIndex + 1) % wayPoint.Count;

        // Calcular un nuevo objetivo alrededor del waypoint
        Vector3 randomOffset = new Vector3(
            Random.Range(-waypointAreaRadius, waypointAreaRadius),
            0,
            Random.Range(-waypointAreaRadius, waypointAreaRadius)
        );
        targetPosition = wayPoint[currentWaypointIndex].position + randomOffset;
    }

    private void UpdateAnimator()
    {
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
        if (wayPoint == null || wayPoint.Count == 0) return;

        // Calcular un punto aleatorio dentro del área alrededor del waypoint actual
        Vector3 randomOffset = new Vector3(
            Random.Range(-waypointAreaRadius, waypointAreaRadius),
            0,
            Random.Range(-waypointAreaRadius, waypointAreaRadius)
        );
        targetPosition = wayPoint[currentWaypointIndex].position + randomOffset;
    }

}

