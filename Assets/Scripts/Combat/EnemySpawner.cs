using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    public int maxSpawnNumber = 10;
    public float spawnInterval = 2f;
    public GameObject enemyPrefab;

    private int currentSpawned = 0;
    private int remainingEnemies;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        remainingEnemies = maxSpawnNumber;
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (currentSpawned < maxSpawnNumber)
        {
            Vector3 randomPoint = GetRandomPointInTrigger();

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                Instantiate(enemyPrefab, hit.position, Quaternion.identity);
                currentSpawned++;
            }
            else
            {
                SpawnEnemy();
            }
        }
        else
        {
            CancelInvoke(nameof(SpawnEnemy));
            Debug.Log("Level Completed");
        }
    }

    Vector3 GetRandomPointInTrigger()
    {
        Bounds bounds = GetComponent<BoxCollider>().bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, transform.position.y, randomZ);
    }

    public void OnEnemyKilled()
    {
        remainingEnemies--;
        Debug.Log("Remaining Enemies: " + remainingEnemies);

        if (remainingEnemies <= 0)
        {
            Debug.Log("All zombies killed.");
        }
    }

    public bool IsLastEnemy()
    {
        return remainingEnemies == 1;
    }

}
