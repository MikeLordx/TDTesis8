using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTower : MonoBehaviour
{
    [SerializeField] public BulletPooler bulletPool;
    [SerializeField] public Transform firePoint;
    [SerializeField] public float fireRate = 2.5f;
    [SerializeField] public float range = 9f;
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
        GameObject spell = bulletPool.GetBullet();
        spell.transform.position = firePoint.position;
        spell.transform.rotation = firePoint.rotation;

        MageSpell mageSpell = spell.GetComponent<MageSpell>();
        mageSpell.SetTarget(target);

        float distanceToTarget = Vector3.Distance(firePoint.position, target.position);
        float lifetime = Mathf.Max(5f, distanceToTarget / mageSpell.speed + 2f);
        StartCoroutine(ReturnSpellAfterTime(spell, lifetime));
    }

    IEnumerator ReturnSpellAfterTime(GameObject spell, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        bulletPool.ReturnBullet(spell);
    }
}
