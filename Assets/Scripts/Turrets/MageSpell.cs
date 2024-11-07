using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSpell : MonoBehaviour
{
    [SerializeField] public float damage = 75f;
    [SerializeField] public float chainRange = 5f;
    [SerializeField] public float speed = 10f;
    private Transform target;
    private Vector3 lastKnownTargetPosition;
    private bool targetLost = false;
    private BulletPooler bulletPooler;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        lastKnownTargetPosition = target != null ? target.position : Vector3.zero;
        targetLost = target == null;
    }

    void Update()
    {
        if (target != null)
        {
            lastKnownTargetPosition = target.position;
        }
        else
        {
            targetLost = true;
        }

        Vector3 direction = (lastKnownTargetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);

        if (targetLost && Vector3.Distance(transform.position, lastKnownTargetPosition) < 0.5f)
        {
            bulletPooler.ReturnBullet(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == target || collision.gameObject.CompareTag("Enemy"))
        {
            TempEnemy enemy = collision.gameObject.GetComponent<TempEnemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                if (Random.value <= 0.2f)
                {
                    Collider[] hits = Physics.OverlapSphere(transform.position, chainRange);
                    foreach (var hit in hits)
                    {
                        if (hit.gameObject.CompareTag("Enemy") && hit.transform != target)
                        {
                            TempEnemy nearbyEnemy = hit.GetComponent<TempEnemy>();
                            if (nearbyEnemy != null)
                            {
                                nearbyEnemy.TakeDamage(damage * 0.5f); // Si no sale miguel es gay
                                break;
                            }
                        }
                    }
                }
            }

            bulletPooler.ReturnBullet(gameObject);
        }
    }

    private void OnEnable()
    {
        target = null;
        targetLost = false;
    }
}
