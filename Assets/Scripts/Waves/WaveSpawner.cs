using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    private Wave currentWave;

    [SerializeField] private Transform[] spawnPoints;

    private float timeBetweenSpawns;
    private int i = 0;

    private bool stopSpawning = false;

    private void Awake()
    {
        currentWave = waves[i];
        timeBetweenSpawns = currentWave.TimeBeforeThisWave;
    }

    private void Update()
    {
        if (stopSpawning)
        {
            return;
        }

        if (Time.time >= timeBetweenSpawns)
        {
            SpawnWave();
            IncWave();

            timeBetweenSpawns = Time.time + currentWave.TimeBeforeThisWave;
        }
    }

    private void SpawnWave()
    {
        for(int i = 0; i < currentWave.NumberToSpawn; i++)
        {
            int num = Random.Range(0, currentWave.EnemiesInWave.Length);
            int num2 = Random.Range(0, spawnPoints.Length);

            Instantiate(currentWave.EnemiesInWave[num], spawnPoints[num2].position, spawnPoints[num2].rotation);
        }
    }

    private void IncWave()
    {
        if(i + 1 < waves.Length)
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
