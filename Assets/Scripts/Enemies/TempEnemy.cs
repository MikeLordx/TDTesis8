using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : MonoBehaviour
{
    [SerializeField] public float health = 100f;
    [SerializeField] public int rewardAmount = 10;
    WaveSpawner waveSpawner;
    [SerializeField] public GameObject coinPrefab;

    public void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0f);
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        DropCoins();
        //waveSpawner.EnemyKilled();
        GameManager.instance.AddCoins(rewardAmount);
        Destroy(gameObject);
    }

    void DropCoins()
    {
        GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = coin.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
            rb.AddForce(randomDirection * 5f, ForceMode.Impulse);
            Vector3 randomTorque = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            rb.AddTorque(randomTorque, ForceMode.Impulse);
            rb.drag = 1f;
            rb.angularDrag = 0.5f;
        }
    }
}