using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementTower : MonoBehaviour
{
    public List<Transform> wayPoint;
    public float playerDetectionRange = 5f;
    public float towerDetectionRange = 1.5f;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private int currentWaypointIndex = 0;
    private Transform player;
    private Transform targetTower;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        InitializeWaypoints();

        animator.SetBool("isWalking", true);
        Walking();
    }

    void Update()
    {
        bool hasTarget = false;
        targetTower = FindClosestTower();
        if (targetTower != null && Vector3.Distance(transform.position, targetTower.position) <= towerDetectionRange)
        {
            navMeshAgent.SetDestination(targetTower.position);
            hasTarget = true;
        }
        if (!hasTarget && player != null && Vector3.Distance(transform.position, player.position) <= playerDetectionRange)
        {
            navMeshAgent.SetDestination(player.position);
            hasTarget = true;
        }
        if (!hasTarget)
        {
            Walking();
        }
        animator.SetBool("isWalking", navMeshAgent.velocity.magnitude > 0.1f);
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
    private void InitializeWaypoints()
    {
        if (wayPoint == null || wayPoint.Count == 0)
        {
            Debug.LogWarning("No hay waypoints asignados. Buscando en la escena...");
            GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
            foreach (GameObject obj in waypointObjects)
            {
                wayPoint.Add(obj.transform);
            }
            wayPoint.Sort((a, b) => a.name.CompareTo(b.name));
        }
    }

    private void Walking()
    {
        if (wayPoint == null || wayPoint.Count == 0)
        {
            Debug.LogWarning("La lista de waypoints está vacía o no asignada.");
            return;
        }
        if (wayPoint[currentWaypointIndex] == null)
        {
            Debug.LogWarning($"El waypoint en el índice {currentWaypointIndex} no está asignado.");
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
