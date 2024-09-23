using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{
    // Hacer que EnemiesAlive sea no estática
    public int enemiesAlive = 0;

    public Wave[] waves;
    public SpawnPointWaypoints[] spawnPointsWithWaypoints; // Array de puntos de spawn con waypoints

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    public TMP_Text countdownText;

    private int waveIndex = 0;

    private void Update()
    {
        // Usar la variable de instancia en lugar de la estática
        if (enemiesAlive > 0)
        {
            return;
        }

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
                // Selecciona un spawn point aleatorio para cada enemigo
                int spawnPointIndex = Random.Range(0, spawnPointsWithWaypoints.Length);
                Debug.Log($"Spawning enemy {wave.enemies[i].name} at spawn point {spawnPointIndex} from {gameObject.name}");

                // Spawnear enemigo en ese punto
                SpawnEnemy(wave.enemies[i], spawnPointsWithWaypoints[spawnPointIndex]);

                yield return new WaitForSeconds(1f / wave.spawnRate); // Esperar antes de spawnear el siguiente enemigo
            }
        }

        waveIndex++;
    }


    void SpawnEnemy(GameObject enemyPrefab, SpawnPointWaypoints spawnPointWithWaypoints)
    {
        // Instanciar el enemigo en la posición del spawn point seleccionado
        GameObject enemy = Instantiate(enemyPrefab, spawnPointWithWaypoints.spawnPoint.position, spawnPointWithWaypoints.spawnPoint.rotation);

        // Asignar los waypoints al enemigo
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.wayPoint = new List<Transform>(spawnPointWithWaypoints.waypoints);
        }

        // Aumentar la cuenta de enemigos vivos para este spawner
        enemiesAlive++;
    }

    // Asegurarse de que la cuenta de enemigos se reduce cuando uno muere (esto puede depender de cómo manejas la muerte de los enemigos)
    public void EnemyKilled()
    {
        enemiesAlive--;
    }
}
