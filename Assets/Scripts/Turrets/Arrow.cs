using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] public float damage = 30f;
    [SerializeField] private BulletPooler bulletPooler;

    public void SetBulletPooler(BulletPooler pooler)
    {
        bulletPooler = pooler;
    }

    private void OnCollisionEnter(Collision collision)
    {
        TempEnemy enemy = collision.gameObject.GetComponent<TempEnemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}