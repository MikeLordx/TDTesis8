using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> wayPoint;
    public float playerDetectionRange = 5f;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private int currentWaypointIndex = 0;
    private Transform player;
    private PlayerHealth playerHealth;
    private bool chasingPlayer = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
        animator.SetBool("isWalking", true);
        Walking();
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
                Walking();
            }
        }
        else
        {
            chasingPlayer = false;
            Walking();
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

    private void Walking()
    {
        if (wayPoint == null || wayPoint.Count == 0)
        {
            return;
        }

        float distanceToWaypoint = Vector3.Distance(wayPoint[currentWaypointIndex].position, transform.position);

        if (distanceToWaypoint <= 2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoint.Count;
        }
        if (!chasingPlayer)
        {
            navMeshAgent.SetDestination(wayPoint[currentWaypointIndex].position);
        }
    }
}
