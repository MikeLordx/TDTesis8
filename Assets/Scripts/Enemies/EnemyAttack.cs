using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
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
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRange && attackCooldown <= 0f)
        {
            StartCoroutine(PerformAttack());
            attackCooldown = timeBetweenAttacks;
        }

        attackCooldown -= Time.deltaTime;
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