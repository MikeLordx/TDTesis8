using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage = 30f;
    private BulletPooler bulletPooler;

    public void SetBulletPooler(BulletPooler pooler)
    {
        bulletPooler = pooler;
    }

    private void OnCollisionEnter(Collision collision)
    {
        TempEnemy enemy = collision.collider.GetComponent<TempEnemy>();
        if (enemy != null)
        { 
            enemy.TakeDamage(damage);
            Debug.Log("Impactaron a yo con: " + damage + "de daño");
        }
        else
        {
            Debug.Log("No es yo o algun enemigo.");
        }
        if (bulletPooler != null)
        {
            bulletPooler.ReturnBullet(gameObject);
        }
    }
}