using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> wayPoint;
    public float playerDetectionRange = 5f;

    private NavMeshAgent navMeshAgent;
    private int currentWaypointIndex = 0;
    private Transform player;
    private PlayerHealth playerHealth;
    private bool chasingPlayer = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }

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