using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Transform))]
public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public GameObject obstacleFolder;
    public List<ObstacleEntry> obstaclePrefabs = new List<ObstacleEntry>();
    public List<GameObject> powerupPrefabs = new List<GameObject>();
    public List<GameObject> coinPrefabs = new List<GameObject>();

    [Header("Spawn Area")]
    public float minX = 10f;
    public float maxX = 300f;
    public float minZ = -15f;
    public float maxZ = 15f;
    public float spawnY = 0f;

    [Header("Replacement Height Overrides")]
    public float powerupY = 0f;
    public float coinY = 0f;

    [Header("Randomization")]
    public bool randomRotation = false;
    public Vector3 rotationMin = new Vector3(0, 0, 0);
    public Vector3 rotationMax = new Vector3(0, 360, 0);

    public int seed = 42;
    public int maxPlacementAttempts = 1000;

    [Range(0f, 1f)]
    public float powerupPercentage = 0.05f;

    [Range(0f, 1f)]
    public float coinPercentage = 0.1f;

    public NavMeshSurface navMeshSurface;

    private List<GameObject> spawnedObstacles = new List<GameObject>();

    void Start()
    {
        if (obstacleFolder == null)
        {
            obstacleFolder = new GameObject("Obstacles");
            obstacleFolder.transform.SetParent(transform);
        }

        SpawnWithPoisson();
        ReplaceWithPowerups();
        ReplaceWithCoins();
        BuildNavMesh();
    }

    void SpawnWithPoisson()
    {
        if (obstaclePrefabs.Count == 0)
        {
            Debug.LogWarning("ObstacleSpawner: No obstacle prefabs assigned.");
            return;
        }

        Random.State previousState = Random.state;
        Random.InitState(seed);

        List<PlacedObstacle> placed = new List<PlacedObstacle>();

        int attempts = 0;

        while (attempts < maxPlacementAttempts)
        {
            attempts++;

            // Pick a random obstacle first for variable radius spacing
            ObstacleEntry entry = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
            if (entry == null || entry.prefab == null)
                continue;

            float radius = entry.radius;

            // Random candidate position inside spawn bounds
            Vector3 candidate = new Vector3(
                Random.Range(minX, maxX),
                spawnY,
                Random.Range(minZ, maxZ)
            );

            // Check minimum distance from existing obstacles, considering both radii
            bool valid = true;
            foreach (var p in placed)
            {
                float minDist = radius + p.radius;
                if (Vector2.Distance(new Vector2(candidate.x, candidate.z),
                                     new Vector2(p.position.x, p.position.z)) < minDist)
                {
                    valid = false;
                    break;
                }
            }

            if (!valid)
                continue;

            // Valid placement, instantiate obstacle
            Vector3 finalPos = candidate + entry.offset;

            Quaternion rot = randomRotation
                ? Quaternion.Euler(
                    Random.Range(rotationMin.x, rotationMax.x),
                    Random.Range(rotationMin.y, rotationMax.y),
                    Random.Range(rotationMin.z, rotationMax.z))
                : entry.prefab.transform.rotation;

            GameObject obj = Instantiate(entry.prefab, finalPos, rot, obstacleFolder.transform);
            spawnedObstacles.Add(obj);

            placed.Add(new PlacedObstacle { position = finalPos, radius = radius });
        }

        Random.state = previousState;

        Debug.Log($"ObstacleSpawner: Placed {spawnedObstacles.Count} obstacles using Poisson sampling.");
    }

    void ReplaceWithPowerups()
    {
        if (powerupPrefabs.Count == 0) return;

        int count = Mathf.RoundToInt(spawnedObstacles.Count * powerupPercentage);
        HashSet<int> used = new HashSet<int>();

        for (int i = 0; i < count; i++)
        {
            int index = GetRandomValidIndex(used);
            if (index < 0) break;

            used.Add(index);

            GameObject old = spawnedObstacles[index];
            if (old == null) continue;

            Vector3 newPos = new Vector3(old.transform.position.x, powerupY, old.transform.position.z);

            Destroy(old);

            int r = Random.Range(0, powerupPrefabs.Count);
            GameObject replacement = Instantiate(powerupPrefabs[r], newPos, Quaternion.identity, obstacleFolder.transform);
            replacement.name = powerupPrefabs[r].name;

            spawnedObstacles[index] = replacement;
        }
    }

    void ReplaceWithCoins()
    {
        if (coinPrefabs.Count == 0) return;

        int count = Mathf.RoundToInt(spawnedObstacles.Count * coinPercentage);
        HashSet<int> used = new HashSet<int>();
        transform.root.Find("GameProgress").GetComponent<GameProgress>().StartScene(SceneManager.GetActiveScene().buildIndex, count);
        for (int i = 0; i < count; i++)
        {
            int index = GetRandomValidIndex(used);
            if (index < 0) break;

            used.Add(index);

            GameObject old = spawnedObstacles[index];
            if (old == null) continue;

            Vector3 newPos = new Vector3(old.transform.position.x, coinY, old.transform.position.z);

            Destroy(old);

            int r = Random.Range(0, coinPrefabs.Count);
            GameObject replacement = Instantiate(coinPrefabs[r], newPos, Quaternion.identity, obstacleFolder.transform);
            replacement.name = coinPrefabs[r].name;

            spawnedObstacles[index] = replacement;
        }
    }

    int GetRandomValidIndex(HashSet<int> used)
    {
        for (int tries = 0; tries < spawnedObstacles.Count * 2; tries++)
        {
            int index = Random.Range(0, spawnedObstacles.Count);
            if (!used.Contains(index) && spawnedObstacles[index] != null)
                return index;
        }
        return -1;
    }

    void BuildNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
            Debug.Log("NavMesh built.");
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

    // Helper struct to store placed obstacles info for distance checking
    private struct PlacedObstacle
    {
        public Vector3 position;
        public float radius;
    }
}
