using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarProjectile : MonoBehaviour
{
    [SerializeField] public float damage = 100f;
    [SerializeField] public float explosionRadius = 3f;
    [SerializeField] public float speed = 15f;
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
            Explode();
            bulletPooler.ReturnBullet(gameObject);
        }
    }

    private void Explode()
    {
        float currentRadius = explosionRadius;
        if (Random.value <= 0.15f) //Según meti probabilidad nose si salga
        {
            currentRadius *= 2;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, currentRadius);
        foreach (var hit in hits)
        {
            TempEnemy enemy = hit.GetComponent<TempEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    private void OnEnable()
    {
        target = null;
        targetLost = false;
    }
}
