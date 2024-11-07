using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarTower : MonoBehaviour
{
    [SerializeField] public BulletPooler bulletPool;
    [SerializeField] public Transform firePoint;
    [SerializeField] public float fireRate = 3f;
    [SerializeField] public float range = 8f;
    [SerializeField] private float nextFireTime = 0f;
    [SerializeField] private Transform target;

    void Update()
    {
        FindTarget();

        if (target != null && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
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
        GameObject mortarProjectile = bulletPool.GetBullet();
        mortarProjectile.transform.position = firePoint.position;
        mortarProjectile.transform.rotation = firePoint.rotation;

        MortarProjectile mortar = mortarProjectile.GetComponent<MortarProjectile>();
        mortar.SetTarget(target);
        float distanceToTarget = Vector3.Distance(firePoint.position, target.position);
        float lifetime = Mathf.Max(5f, distanceToTarget / mortar.speed + 2f);
        StartCoroutine(ReturnProjectileAfterTime(mortarProjectile, lifetime));
    }

    IEnumerator ReturnProjectileAfterTime(GameObject projectile, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        bulletPool.ReturnBullet(projectile);
    }
}
