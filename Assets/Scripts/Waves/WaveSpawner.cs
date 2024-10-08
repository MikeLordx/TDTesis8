using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public int enemiesAlive = 0;

    public Wave[] waves;
    public SpawnPointWaypoints[] spawnPointsWithWaypoints;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    public TMP_Text countdownText;

    private int waveIndex = 0;

    private void Update()
    {
        /*if (enemiesAlive > 0)
        {
            if(totalEnemiesAlive > 0)
            {
                return;
            }
        }*/

        if (countdown <= 0)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        countdownText.text = string.Format("{0:00.00}", countdown);
    }

    IEnumerator SpawnWave()
    {
        Debug.Log($"Spawning wave {waveIndex} from {gameObject.name}");
        Wave wave = waves[waveIndex];

        for (int i = 0; i < wave.enemies.Length; i++)
        {
            for (int j = 0; j < wave.enemiesCount[i]; j++)
            {
                int spawnPointIndex = Random.Range(0, spawnPointsWithWaypoints.Length);
                Debug.Log($"Spawning enemy {wave.enemies[i].name} at spawn point {spawnPointIndex} from {gameObject.name}");

                SpawnEnemy(wave.enemies[i], spawnPointsWithWaypoints[spawnPointIndex]);

                yield return new WaitForSeconds(1f / wave.spawnRate);
            }
        }

        waveIndex++;
    }

    void SpawnEnemy(GameObject enemyPrefab, SpawnPointWaypoints spawnPointWithWaypoints)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPointWithWaypoints.spawnPoint.position, spawnPointWithWaypoints.spawnPoint.rotation);

        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.wayPoint = new List<Transform>(spawnPointWithWaypoints.waypoints);
        }

        enemiesAlive++;
    }

    public void EnemyKilled()
    {
        enemiesAlive--;
    }
}
