using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : MonoBehaviour
{
    [SerializeField] public float health = 100f;
    WaveSpawner waveSpawner;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        waveSpawner.EnemyKilled();
        Destroy(gameObject);
    }
}