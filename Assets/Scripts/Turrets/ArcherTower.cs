using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : MonoBehaviour
{
    [SerializeField] public BulletPooler bulletPool;
    [SerializeField] public Transform firePoint;
    [SerializeField] public float fireRate = 1.5f;
    [SerializeField] public float range = 10f;
    [SerializeField] private float nextFireTime = 0f;
    [SerializeField] private Transform target;
    [SerializeField] private float damage = 30f;
    private bool maxLevel = false;

    void Update()
    {
        FindTarget();

        if (target != null && Time.time >= nextFireTime)
        {
            if (Vector3.Distance(transform.position, target.position) <= range)
            {
                Fire();
                nextFireTime = Time.time + fireRate;
            }
            else
            {
                target = null;
            }
        }
    }

    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                target = hit.transform;
                break;
            }
        }
    }

    void Fire()
    {
        GameObject arrow = bulletPool.GetBullet();
        arrow.transform.position = firePoint.position;
        arrow.transform.rotation = firePoint.rotation;
        float currentDamage = damage;

        if (Random.value <= 0.1f)
        {
            currentDamage *= 1.5f;
        }

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.SetBulletPooler(bulletPool);
        arrowScript.damage = currentDamage;
        arrowScript.SetTarget(target);

        if (maxLevel)
        {
            RaycastHit[] hits = Physics.RaycastAll(firePoint.position, (target.position - firePoint.position).normalized, range);
            int penetratedEnemies = 0;
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    TempEnemy enemy = hit.collider.GetComponent<TempEnemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(currentDamage);
                        penetratedEnemies++;
                        if (penetratedEnemies >= 2) break;
                    }
                }
            }
        }
    }
}
