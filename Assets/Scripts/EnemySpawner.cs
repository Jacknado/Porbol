using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LightTransport;


public class EnemySpawner : MonoBehaviour
{
    public Transform player;
    public List<GameObject> enemyList = new List<GameObject>();

    public float startingInterval;
    public float intervalDecrament;
    public int enemiesPerSpawn = 1;
    public Transform enemyParent;
    public float spwanDistance;
    private float timer;
    public float minZ;
    public float maxZ;
    public float minInterval;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = startingInterval;
    }

    private GameObject selectEnemy()
    {
        return enemyList[Random.Range(0, enemyList.Count)];
    }
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            for (int i = 1; i <= enemiesPerSpawn; i++)
            {
                Instantiate(selectEnemy(), new Vector3(player.transform.position.x + spwanDistance, player.transform.position.y, Random.Range(minZ, maxZ)), player.rotation, enemyParent);
            }
            if (startingInterval - intervalDecrament <= minInterval)
                startingInterval -= intervalDecrament;
            timer = startingInterval;
        }
    }
}
