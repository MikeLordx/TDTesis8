using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    private Wave currentWave;

    [SerializeField] private List<SpawnPointWaypoints> spawnPointWaypoints; // Lista que asocia puntos de spawn con waypoints

    private int waveIndex = 0;
    private bool stopSpawning = false;

    private int currentEnemyIndex = 0; // Para llevar control de qué enemigo spawnear
    private int enemiesSpawnedInCurrentWave = 0;
    private float nextSpawnTime = 0f; // Controlar el tiempo del siguiente spawn
    private float nextWaveTime = 0f; // Controlar el tiempo hasta la próxima oleada

    private void Awake()
    {
        currentWave = waves[waveIndex];
        nextWaveTime = Time.time + currentWave.TimeBeforeThisWave; // Establecer el tiempo de la próxima oleada
    }

    private void Update()
    {
        if (stopSpawning)
        {
            return;
        }

        // Verificar si es el momento de iniciar la nueva oleada
        if (Time.time >= nextWaveTime)
        {
            // Si aún quedan enemigos por spawnear en la oleada actual
            if (enemiesSpawnedInCurrentWave < TotalEnemiesInWave())
            {
                if (Time.time >= nextSpawnTime)
                {
                    SpawnEnemiesSimultaneously(); // Spawnear todos los enemigos de la oleada

                }
            }
            else
            {
                IncWave(); // Avanza a la siguiente oleada
            }
        }
    }

    // Devuelve el número total de enemigos a spawnear en la oleada
    private int TotalEnemiesInWave()
    {
        int totalEnemies = 0;
        foreach (var enemyData in currentWave.EnemiesToSpawn)
        {
            totalEnemies += enemyData.quantity; // Sumar la cantidad de cada tipo de enemigo
        }
        return totalEnemies;
    }

    private void SpawnEnemiesSimultaneously()
    {
        // Iterar sobre todos los tipos de enemigos a spawnear en la oleada actual
        foreach (var enemyData in currentWave.EnemiesToSpawn)
        {
            // Verificar que el índice del punto de spawn sea válido
            if (enemyData.spawnPointIndex < 0 || enemyData.spawnPointIndex >= spawnPointWaypoints.Count)
            {
                Debug.LogError("Índice de punto de spawn fuera de rango.");
                continue;
            }

            // Verificar que el prefab del enemigo y el punto de spawn no sean nulos
            if (enemyData.enemyPrefab != null && spawnPointWaypoints[enemyData.spawnPointIndex] != null)
            {
                // Spawnear la cantidad de enemigos especificada en enemyData.quantity
                for (int i = 0; i < enemyData.quantity; i++)
                {
                    // Instanciar cada enemigo en el punto de spawn correcto
                    GameObject enemyInstance = Instantiate(enemyData.enemyPrefab, spawnPointWaypoints[enemyData.spawnPointIndex].spawnPoint.position, spawnPointWaypoints[enemyData.spawnPointIndex].spawnPoint.rotation);

                    // Asignar waypoints o rutas al enemigo instanciado
                    EnemyMovement enemyMovement = enemyInstance.GetComponent<EnemyMovement>();
                    if (enemyMovement != null)
                    {
                        enemyMovement.wayPoint = spawnPointWaypoints[enemyData.spawnPointIndex].waypoints;
                    }

                    enemiesSpawnedInCurrentWave++; // Incrementa el contador de enemigos spawneados
                }
            }
            else
            {
                Debug.LogError("Prefab del enemigo o punto de spawn es nulo.");
            }
        }
    }

    private void IncWave()
    {
        // Asegurarse de avanzar solo si hay más oleadas disponibles
        if (waveIndex + 1 < waves.Length)
        {
            waveIndex++;
            currentWave = waves[waveIndex];
            currentEnemyIndex = 0; // Reinicia el contador de tipos de enemigos
            enemiesSpawnedInCurrentWave = 0; // Reinicia el contador de enemigos de la oleada
            nextWaveTime = Time.time + currentWave.TimeBeforeThisWave; // Establece el tiempo para la próxima oleada
        }
        else
        {
            stopSpawning = true; // No más oleadas, detener el spawneo
            Debug.Log("Todas las oleadas han sido completadas.");
        }
    }
}