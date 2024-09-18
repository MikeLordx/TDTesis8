using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPointStatus
{
    public SpawnPointWaypoints spawnPointWaypoints; // Referencia al punto de spawn y sus waypoints
    public float nextSpawnTime; // Tiempo para el próximo spawn en este punto
    public int enemiesSpawnedInCurrentWave; // Contador de enemigos ya spawneados en este punto
    public int totalEnemiesToSpawn; // Cantidad total de enemigos que deben spawnearse en este punto
    public EnemySpawnData currentEnemyData; // Datos del enemigo a spawnear
}
