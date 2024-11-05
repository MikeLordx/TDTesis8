using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackEnemy : MonoBehaviour
{
    [SerializeField] public float damage = 10f;
    [SerializeField] public float timeBetweenAttacks = 2f;
    [SerializeField] public float attackRange = 1.5f;
    [SerializeField] public float attackDelay = 0.5f;

    private float attackCooldown;
    private Transform target;

    private void Start()
    {
        attackCooldown = 0f;
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;
        Transform targetTower = FindClosestTower();
        if (targetTower != null && Vector3.Distance(transform.position, targetTower.position) <= attackRange)
        {
            target = targetTower;
            if (attackCooldown <= 0f)
            {
                StartCoroutine(PerformAttack(target));
                attackCooldown = timeBetweenAttacks;
            }
        }
        else
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                target = player;
                if (attackCooldown <= 0f)
                {
                    StartCoroutine(PerformAttack(target));
                    attackCooldown = timeBetweenAttacks;
                }
            }
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

    private IEnumerator PerformAttack(Transform target)
    {
        yield return new WaitForSeconds(attackDelay);
        if (target != null)
        {
            if (target.CompareTag("Tower"))
            {
                TowerHealth towerHealth = target.GetComponent<TowerHealth>();
                if (towerHealth != null)
                {
                    towerHealth.TakeDamage(damage);
                }
            }
            else if (target.CompareTag("Player"))
            {
                PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
        }
    }
}