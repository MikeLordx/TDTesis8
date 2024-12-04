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
    
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private GameObject selectedBulletPrefab;

    void Fire()
    {
        if (selectedBulletPrefab == null)
        {
            Debug.LogWarning("No hay bala");
            return;
        }
        GameObject bullet = Instantiate(selectedBulletPrefab, firePoint.position, firePoint.rotation);

        float currentDamage = damage;
        if (Random.value <= 0.1f)
        {
            currentDamage *= 1.5f;
        }
        Arrow arrowScript = bullet.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.damage = currentDamage;
            arrowScript.SetTarget(target);
        }
        if (fireSound != null)
        {
            AudioManager.instance.PlaySFX(fireSound);
        }
    }

}
