using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    private Wave currentWave;

    [SerializeField] private List<SpawnPointWaypoints> spawnPointsWithWaypoints;

    private float timeBetweenWaves;
    private int i = 0;

    private float timeBetweenSpawns = 1f;
    private float timeSinceLastSpawn = 0f;
    private int enemiesSpawned = 0;
    private bool isSpawningWave = false;
    private bool stopSpawning = false;

    private void Awake()
    {
        currentWave = waves[i];
        timeBetweenWaves = currentWave.TimeBeforeThisWave;
    }

    private void Update()
    {
        if (stopSpawning)
        {
            return;
        }

        if (isSpawningWave)
        {
            if (Time.time >= timeSinceLastSpawn + timeBetweenSpawns && enemiesSpawned < currentWave.NumberToSpawn)
            {
                SpawnEnemy();
                enemiesSpawned++;
                timeSinceLastSpawn = Time.time;
            }

            if (enemiesSpawned >= currentWave.NumberToSpawn)
            {
                isSpawningWave = false;
                IncWave();
                timeBetweenWaves = Time.time + currentWave.TimeBeforeThisWave;
            }
        }
        else if (Time.time >= timeBetweenWaves)
        {
            StartWave();
        }
    }

    private void SpawnEnemy()
    {
        int enemyIndex = Random.Range(0, currentWave.EnemiesInWave.Length);
        int spawnPointIndex = Random.Range(0, spawnPointsWithWaypoints.Count);

        GameObject enemy = Instantiate(currentWave.EnemiesInWave[enemyIndex], spawnPointsWithWaypoints[spawnPointIndex].spawnPoint.position, spawnPointsWithWaypoints[spawnPointIndex].spawnPoint.rotation);

        // Asignar la lista de waypoints correspondiente al spawn point
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.wayPoint = spawnPointsWithWaypoints[spawnPointIndex].waypoints;
        }
    }

    private void StartWave()
    {
        enemiesSpawned = 0;
        isSpawningWave = true;
        timeSinceLastSpawn = Time.time;
    }

    private void IncWave()
    {
        if (i + 1 < waves.Length)
        {
            i++;
            currentWave = waves[i];
        }
        else
        {
            stopSpawning = true;
        }
    }
}
