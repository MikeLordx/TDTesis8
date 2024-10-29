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
    private Transform targetTower;

    private void Start()
    {
        attackCooldown = 0f;
        targetTower = GameObject.FindGameObjectWithTag("Tower").transform;
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;
        if (targetTower != null && Vector3.Distance(transform.position, targetTower.position) <= attackRange)
        {
            if (attackCooldown <= 0f)
            {
                StartCoroutine(PerformAttack());
                attackCooldown = timeBetweenAttacks;
            }
        }
    }

    private IEnumerator PerformAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        if (targetTower != null)
        {
            TowerHealth towerHealth = targetTower.GetComponent<TowerHealth>();
            if (towerHealth != null)
            {
                towerHealth.TakeDamage(damage);
            }
        }
    }
}