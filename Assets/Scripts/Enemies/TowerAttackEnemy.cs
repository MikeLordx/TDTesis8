using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackEnemy : MonoBehaviour
{
    [Header("Attack Settings")]
    [Tooltip("Daño infligido por el enemigo por ataque.")]
    public float damage = 10f;

    [Tooltip("Tiempo entre cada ataque en segundos.")]
    public float timeBetweenAttacks = 2f;

    [Tooltip("Rango en el que el enemigo puede atacar la torre.")]
    public float attackRange = 1.5f;

    [Tooltip("Delay antes de aplicar el daño a la torre.")]
    public float attackDelay = 0.5f;

    private float attackCooldown;
    private Transform targetTower;

    private void Start()
    {
        attackCooldown = 0f;
        targetTower = GameObject.FindGameObjectWithTag("Tower")?.transform;
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