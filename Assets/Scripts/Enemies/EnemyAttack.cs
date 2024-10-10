using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [Tooltip("Damage dealt by the enemy per attack.")]
    public float damage = 10f;

    [Tooltip("Time between each attack in seconds.")]
    public float timeBetweenAttacks = 2f;

    [Tooltip("The range within which the enemy can attack the player.")]
    public float attackRange = 1.5f;

    [Tooltip("The delay before the attack is applied to the player.")]
    public float attackDelay = 0.5f;

    private float attackCooldown;
    private Transform target;

    private void Start()
    {
        attackCooldown = 0f;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;
        if (target != null && Vector3.Distance(transform.position, target.position) <= attackRange)
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
        if (target != null)
        {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}