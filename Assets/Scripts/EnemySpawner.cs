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
    public List<EnemyChance> enemies;

    public Dictionary<GameObject, float> enemyPrefab = new Dictionary<GameObject, float>();
    public Transform player;
    public float spawnDistanceAhead = 10f;
    public float zSpawnRange = 15f;
    public int enemiesPerWave = 2;
    public float spawnIntervalX = 50f;
    private float nextSpawnX = 50f;

    void Update()
    {
        if (player.position.x >= nextSpawnX)
        {
            SpawnEnemies();
            nextSpawnX += spawnIntervalX;
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            float zOffset = Random.Range(-zSpawnRange, zSpawnRange);
            Vector3 spawnPos = new Vector3(player.position.x + spawnDistanceAhead, 0, zOffset);

            GameObject enemy = Instantiate(GetEnemy(), spawnPos, Quaternion.identity);

            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                Enemy follow = enemy.GetComponent<Enemy>();
                if (follow != null)
                    follow.target = player;
            }
        }
    }

    GameObject GetEnemy()
    {
        List<GameObject> enemiesToSpawn = new List<GameObject>();
        foreach(EnemyChance enemy in enemies)
            for(int i = 0; i <= enemy.chance; i++)
                enemiesToSpawn.Add(enemy.enemy);
        return enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)];
    }
}
