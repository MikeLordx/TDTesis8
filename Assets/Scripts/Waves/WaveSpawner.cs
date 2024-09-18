using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    private Wave currentWave;

    [SerializeField] private List<SpawnPointWaypoints> spawnPointWaypoints; // Lista que asocia puntos de spawn con waypoints
    [SerializeField] private float delayBetweenEnemies = 1f; // Tiempo de espera entre cada spawn

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
            // Control del tiempo antes de la siguiente oleada
            if (enemiesSpawnedInCurrentWave < currentWave.EnemiesToSpawn[currentEnemyIndex].quantity)
            {
                if (Time.time >= nextSpawnTime)
                {
                    SpawnEnemy();
                    nextSpawnTime = Time.time + delayBetweenEnemies; // Establece el próximo tiempo de spawn
                }
            }
            else
            {
                // Si ya se han spawneado todos los enemigos de este tipo, avanza al siguiente tipo de enemigo
                currentEnemyIndex++;

                if (currentEnemyIndex >= currentWave.EnemiesToSpawn.Count)
                {
                    // Si ya se spawnearon todos los enemigos de la oleada, pasa a la siguiente oleada
                    IncWave();
                }
                else
                {
                    // Reinicia el contador de enemigos para el siguiente tipo de enemigo
                    enemiesSpawnedInCurrentWave = 0;
                }
            }
        }
    }

    private void SpawnEnemy()
    {
        // Verificar si la lista de puntos de spawn tiene elementos
        if (spawnPointWaypoints.Count == 0)
        {
            Debug.LogError("La lista de puntos de spawn está vacía.");
            return;
        }

        // Obtén el índice del punto de spawn para el enemigo actual
        var enemyData = currentWave.EnemiesToSpawn[currentEnemyIndex];
        int spawnPointIndex = enemyData.spawnPointIndex;

        // Verificar si el índice del punto de spawn es válido
        if (spawnPointIndex < 0 || spawnPointIndex >= spawnPointWaypoints.Count)
        {
            Debug.LogError("Índice de punto de spawn fuera de rango.");
            return;
        }

        // Verifica que no sea null el prefab y el punto de spawn
        if (enemyData.enemyPrefab != null && spawnPointWaypoints[spawnPointIndex] != null)
        {
            // Instancia el enemigo en la posición y rotación del spawn point correspondiente
            GameObject enemyInstance = Instantiate(enemyData.enemyPrefab, spawnPointWaypoints[spawnPointIndex].spawnPoint.position, spawnPointWaypoints[spawnPointIndex].spawnPoint.rotation);

            // Asignar waypoints o rutas al enemigo instanciado
            EnemyMovement enemyMovement = enemyInstance.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                // Asignar los waypoints correspondientes al punto de spawn
                enemyMovement.wayPoint = spawnPointWaypoints[spawnPointIndex].waypoints;
            }

            enemiesSpawnedInCurrentWave++; // Incrementa el contador de enemigos spawneados
        }
        else
        {
            Debug.LogError("Prefab del enemigo o punto de spawn es nulo.");
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