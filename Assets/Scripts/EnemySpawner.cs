using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct EnemyChance
{
    public GameObject enemy;
    public float chance;
}

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyChance> enemies = new List<EnemyChance>();
    public Transform player;
    public float spawnDistanceAhead = 10f;
    public float zSpawnRange = 15f;
    public int enemiesPerWave = 2;
    public float spawnIntervalX = 50f;

    private float nextSpawnX = 50f;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("EnemySpawner: Player reference is missing!");
        }

        if (enemies.Count == 0)
        {
            Debug.LogWarning("EnemySpawner: No enemy types configured!");
        }
    }

    void Update()
    {
        if (player == null)
            return;

        if (player.position.x < 0.1)
        {
            nextSpawnX = 50;
        }

        if (player.position.x >= nextSpawnX && nextSpawnX < 300)
        {
            SpawnEnemies();
            nextSpawnX += spawnIntervalX;
        }
    }

    void SpawnEnemies()
    {
        if (enemies.Count == 0)
            return;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            float zOffset = Random.Range(-zSpawnRange, zSpawnRange);
            Vector3 spawnPos = new Vector3(
                player.position.x + spawnDistanceAhead, 
                0, 
                zOffset
            );

            GameObject enemyPrefab = GetRandomEnemy();
            if (enemyPrefab == null)
                continue;

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.target = player;
            }
            else
            {
                Debug.LogWarning($"EnemySpawner: Spawned enemy {enemy.name} doesn't have Enemy component!");
            }
        }
    }

    GameObject GetRandomEnemy()
    {
        if (enemies.Count == 0)
            return null;

        List<GameObject> weightedList = new List<GameObject>();
        
        foreach (EnemyChance enemy in enemies)
        {
            if (enemy.enemy == null)
                continue;

            int weight = Mathf.Max(1, Mathf.RoundToInt(enemy.chance));
            for (int i = 0; i < weight; i++)
            {
                weightedList.Add(enemy.enemy);
            }
        }

        if (weightedList.Count == 0)
            return null;

        return weightedList[Random.Range(0, weightedList.Count)];
    }

    void OnDrawGizmosSelected()
    {
        if (player == null)
            return;

        Gizmos.color = Color.red;
        Vector3 spawnCenter = new Vector3(player.position.x + spawnDistanceAhead, 0, 0);
        Gizmos.DrawWireCube(spawnCenter, new Vector3(2f, 2f, zSpawnRange * 2f));
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            new Vector3(nextSpawnX, -5, -20), 
            new Vector3(nextSpawnX, 5, 20)
        );
    }
}