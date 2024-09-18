using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public int quantity; // Cantidad de enemigos a spawnear
    public int spawnPointIndex; // Índice del punto de spawn
}

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Waves", order = 1)]
public class Wave : ScriptableObject
{
    [field: SerializeField] public List<EnemySpawnData> EnemiesToSpawn { get; private set; }
    [field: SerializeField] public float TimeBeforeThisWave { get; private set; }
}
