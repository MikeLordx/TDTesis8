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
        TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
        if (!tutorialManager.isTutorialComplete)
        {
            countdownText.text = "Completa el tutorial";
            return;
        }

        // Si ya no hay más olas, detener el spawner.
        if (waveIndex >= waves.Length)
        {
            countdownText.text = "Waves Completed!";
            return;
        }

        // Comienza la cuenta atrás para la próxima ola.
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
        Wave wave = waves[waveIndex];

        for (int i = 0; i < wave.enemies.Length; i++)
        {
            for (int j = 0; j < wave.enemiesCount[i]; j++)
            {
                int spawnPointIndex = Random.Range(0, spawnPointsWithWaypoints.Length);

                SpawnEnemy(wave.enemies[i], spawnPointsWithWaypoints[spawnPointIndex]);

                yield return new WaitForSeconds(1f / wave.spawnRate);
            }
        }

        // Incrementa el índice de la ola.
        waveIndex++;

        // Si se han completado todas las olas, muestra un mensaje.
        if (waveIndex >= waves.Length)
        {
            Debug.Log("All waves completed!");
        }
    }


    void SpawnEnemy(GameObject enemyPrefab, SpawnPointWaypoints spawnPointWithWaypoints)
    {
        Vector3 randomSpawnPosition = new Vector3(
            spawnPointWithWaypoints.spawnPoint.position.x + Random.Range(-5f, 5f),
            spawnPointWithWaypoints.spawnPoint.position.y,
            spawnPointWithWaypoints.spawnPoint.position.z);
        GameObject enemy = Instantiate(enemyPrefab, randomSpawnPosition, spawnPointWithWaypoints.spawnPoint.rotation);

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
