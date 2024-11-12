using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] public float damage = 10f;
    [SerializeField] public float speed = 20f;
    [SerializeField] private BulletPooler bulletPooler;
    private Transform target;
    private Vector3 lastKnownTargetPosition;
    private bool targetLost = false;

    public void SetBulletPooler(BulletPooler pooler)
    {
        bulletPooler = pooler;
    }

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

    private void OnEnable()
    {
        target = null;
        targetLost = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == target || collision.gameObject.CompareTag("Enemy"))
        {
            TempEnemy enemy = collision.gameObject.GetComponent<TempEnemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            bulletPooler.ReturnBullet(gameObject);
        }
    }
}
