using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public List<GameObject> obstaclePrefabs;
    public GameObject powerupPrefab;

    [Header("Spawn Area")]
    public float minX = 10f;
    public float maxX = 300f;
    public float minZ = -15f;
    public float maxZ = 15f;
    public float spawnY = 0f;

    [Header("Spacing & Attempts")]
    public float collisionRadius = 2f;
    public int maxPlacementAttempts = 1000;

    [Header("Powerup Settings")]
    [Range(0f, 1f)]
    public float powerupPercentage = 0.05f;

    [Header("NavMesh")]
    public NavMeshSurface navMeshSurface;

    private List<GameObject> spawnedObstacles = new List<GameObject>();

    void Start()
    {
        SpawnUntilFull();
        ReplaceWithPowerups();
        BuildNavMesh();
    }

    void SpawnUntilFull()
    {
        int failedAttempts = 0;

        while (failedAttempts < maxPlacementAttempts)
        {
            Vector3 pos = new Vector3(
                Random.Range(minX, maxX),
                spawnY,
                Random.Range(minZ, maxZ)
            );

            Collider[] hits = Physics.OverlapSphere(pos, collisionRadius);

            if (hits.Length == 0)
            {
                GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
                GameObject spawned = Instantiate(prefab, pos, Quaternion.identity);
                spawnedObstacles.Add(spawned);
                failedAttempts = 0;
            }
            else
            {
                failedAttempts++;
            }
        }

        Debug.Log($"ObstacleSpawner: Placed {spawnedObstacles.Count} obstacles before area filled.");
    }

    void ReplaceWithPowerups()
    {
        if (powerupPrefab == null || spawnedObstacles.Count == 0) return;

        int powerupCount = Mathf.RoundToInt(spawnedObstacles.Count * powerupPercentage);
        powerupCount = Mathf.Clamp(powerupCount, 0, spawnedObstacles.Count);

        HashSet<int> replacedIndices = new HashSet<int>();

        for (int i = 0; i < powerupCount; i++)
        {
            int index;
            do
            {
                index = Random.Range(0, spawnedObstacles.Count);
            } 
            while (replacedIndices.Contains(index));

            replacedIndices.Add(index);

            GameObject oldObstacle = spawnedObstacles[index];
            Vector3 pos = oldObstacle.transform.position;

            Destroy(oldObstacle);

            GameObject powerup = Instantiate(powerupPrefab, pos, Quaternion.identity);
            powerup.name = powerupPrefab.name;
        }

        Debug.Log($"ObstacleSpawner: Replaced {powerupCount} obstacles with powerups.");
    }

    void BuildNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
            Debug.Log("ObstacleSpawner: NavMesh built.");
        }
        else
        {
            Debug.LogWarning("ObstacleSpawner: NavMeshSurface not assigned, skipping navmesh build.");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            new Vector3((minX + maxX) / 2f, spawnY, (minZ + maxZ) / 2f),
            new Vector3(maxX - minX, 0.1f, maxZ - minZ)
        );
    }
}
