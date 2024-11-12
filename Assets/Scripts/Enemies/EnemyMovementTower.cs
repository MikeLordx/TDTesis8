using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementTower : MonoBehaviour
{
    public List<Transform> wayPoint;
    public float playerDetectionRange = 5f;
    public float towerDetectionRange = 1.5f;

    private NavMeshAgent navMeshAgent;
    private int currentWaypointIndex = 0;
    private Transform player;
    private Transform targetTower; 

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Walking();
    }

    void Update()
    {
        targetTower = FindClosestTower();

        if (targetTower != null && Vector3.Distance(transform.position, targetTower.position) <= towerDetectionRange)
        {
            navMeshAgent.SetDestination(targetTower.position);
        }
        else if (player != null && Vector3.Distance(transform.position, player.position) <= playerDetectionRange)
        {
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            Walking();
        }
    }

    private Transform FindClosestTower()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        Transform closestTower = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject tower in towers)
        {
            float distance = Vector3.Distance(transform.position, tower.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTower = tower.transform;
            }
        }

        return closestTower;
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
        if (targetTower == null && player == null)
        {
            navMeshAgent.SetDestination(wayPoint[currentWaypointIndex].position);
        }
    }
}