using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public int damageAmount = 20;
    public float attackCooldown = 1.0f;
    public AudioClip attackSound;

    private float lastAttackTime = 0f;
    private float externalCooldownEndTime = 0f;
    private Animator animator;
    private Quaternion originalLocalRotation;

    private void Start()
    {
        animator = GetComponent<Animator>();
        originalLocalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (TowerPlacementSystem.IsMenuActiveOrPlacingTower || Time.time < externalCooldownEndTime) return;

        if (Input.GetButtonDown("Fire1") && Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetBool("IsAttacking", true);
            PerformMeleeAttack();
            lastAttackTime = Time.time;
            StartCoroutine(ResetAttack());
        }
    }

    void PerformMeleeAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        AudioManager.instance.PlaySFX(attackSound);

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

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsAttacking", false);
        transform.localRotation = originalLocalRotation;
    }

    public void SetExternalCooldown(float duration)
    {
        externalCooldownEndTime = Time.time + duration;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
