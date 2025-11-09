using UnityEngine;

public class SimpleEnemySpawner : MonoBehaviour
{
    [Header("Prefab del enemigo a generar")]
    public GameObject enemyPrefab;

    [Header("Puntos donde generar enemigos")]
    public Transform[] spawnPoints;

    [Header("Cantidad de enemigos a generar")]
    public int enemiesToSpawn = 5;

    [Header("Intervalo entre cada spawn (0 = inmediato)")]
    public float spawnInterval = 0f;

    // Llamar para generar los enemigos
    void Start()
    {
        SpawnEnemies();
    }
    public void SpawnEnemies()
    {
        if (spawnInterval <= 0f)
        {
            // Spawn inmediato
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnAtRandomPoint();
            }
        }
        else
        {
            // Spawn con intervalo usando coroutine
            StartCoroutine(SpawnWithInterval());
        }
    }

    private void SpawnAtRandomPoint()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    private System.Collections.IEnumerator SpawnWithInterval()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnAtRandomPoint();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
