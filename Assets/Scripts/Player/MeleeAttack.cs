using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public int damageAmount = 20;
    public float attackCooldown = 1.0f;

    private float lastAttackTime = 0f;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= lastAttackTime + attackCooldown)
        {
            PerformMeleeAttack();
            lastAttackTime = Time.time;
        }
    }

    void PerformMeleeAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                TempEnemy enemy = hitCollider.GetComponent<TempEnemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damageAmount);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}