using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] public float damage = 10f;
    [SerializeField] public float timeBetweenAttacks = 2f;
    [SerializeField] public float attackRange = 1.5f;
    [SerializeField] public float attackDelay = 0.5f;
    public AudioClip enemyAttack;

    private float attackCooldown;
    private Transform target;
    private Animator animator;

    private void Start()
    {
        attackCooldown = 0f;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
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
        animator.SetBool("isWalking", false);
        animator.SetTrigger("Attack");
        AudioManager.instance.PlaySFX(enemyAttack);

        yield return new WaitForSeconds(attackDelay);

        if (target != null)
        {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        animator.SetBool("isWalking", true);
    }

}
