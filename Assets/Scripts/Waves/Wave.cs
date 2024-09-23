using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject[] enemies;  // Lista de diferentes tipos de enemigos
    public int[] enemiesCount;    // Cantidad de cada tipo de enemigo
    public float spawnRate;
}