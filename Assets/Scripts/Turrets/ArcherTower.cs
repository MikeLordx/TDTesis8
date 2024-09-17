using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : MonoBehaviour
{
    public BulletPooler bulletPool;
    public Transform firePoint;
    public float fireRate = 1.5f;
    public float range = 10f;
    private float nextFireTime = 0f;
    private Transform target;

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

        Vector3 direction = (target.position - firePoint.position).normalized;
        arrow.GetComponent<Rigidbody>().velocity = direction * 20f;

        StartCoroutine(ReturnArrowAfterTime(arrow, 5f));
    }

    IEnumerator ReturnArrowAfterTime(GameObject arrow, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        bulletPool.ReturnBullet(arrow);
    }
}