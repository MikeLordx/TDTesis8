using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : MonoBehaviour
{
    [SerializeField] public float health = 100f;
    [SerializeField] public int pointsDropped = 10;
    [SerializeField] public int rewardAmount = 10;
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
        GameManager.instance.AddCoins(rewardAmount);
        Destroy(gameObject);
    }
}