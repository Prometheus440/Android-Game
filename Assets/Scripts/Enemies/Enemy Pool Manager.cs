using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    // Prefabs to be spawned
    [SerializeField] private Transform basicEnemyPoolParent;
    [SerializeField] private Transform fastEnemyPoolParent;
    [SerializeField] private Transform tankEnemyPoolParent;

    // Wave variables
    private int waveSize = 10;
    private float timeBetweenWaves = 3f;
    private float spawnDelay = 1f;

    // Pool variables
    private int poolSizePerType = 5;
    private int activeEnemyCount = 0;

    private Queue<GameObject> basicEnemyPool = new Queue<GameObject>();
    private Queue<GameObject> fastEnemyPool = new Queue<GameObject>();
    private Queue<GameObject> tankEnemyPool = new Queue<GameObject>();

    private Camera mainCamera;
    private float minY = 0.5f;
    private int currentWave = 0;
    private bool isSpawningWave = false;

    private void Start()
    {
        mainCamera = Camera.main;
        InitializePools();
        StartCoroutine(WaveSystem());
    }

    void InitializePools()
    {
        // Add enemies to the queues
        foreach (Transform child in basicEnemyPoolParent)
        {
            basicEnemyPool.Enqueue(child.gameObject);
        }
		foreach (Transform child in fastEnemyPoolParent)
		{
			fastEnemyPool.Enqueue(child.gameObject);
		}
		foreach (Transform child in tankEnemyPoolParent)
		{
			tankEnemyPool.Enqueue(child.gameObject);
		}
	}

    IEnumerator WaveSystem()
    {
        while (true)
        {
            currentWave++; // Wave counter
            yield return StartCoroutine(SpawnWave()); // Start spawning enemies
            yield return new WaitUntil(() => AreAllEnemiesDefeated()); // Wait until all enemies in wave are dead
            yield return new WaitForSeconds(timeBetweenWaves); // Gives time to move to next wave
        }
    }

    IEnumerator SpawnWave()
    {
        isSpawningWave = true;
        activeEnemyCount = 0; // Reset per wave

        // Spawn around the camera
        List<Vector2> spawnPos = GenerateSpawnPositions(waveSize);

        // List of a mix of enemy types = 10
        List<int> enemyTypes = GenerateEnemyTypeList(waveSize);

        // Spawn incrementally
        for (int i = 0; i < waveSize; i++)
        {
            GameObject enemy = GetEnemyFromPool(enemyTypes[i]);

            if (enemy != null)
            {
                enemy.transform.position = spawnPos[i];
                enemy.SetActive(true); // Active in heirarchy
                activeEnemyCount++; // Track number of active enemies
            }

            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawningWave = false;
    }

    List<Vector2> GenerateSpawnPositions(int count)
    {
        List<Vector2> positions = new List<Vector2>();

        // Calculate camera so that the enemies spawn around not in view
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = 2f * mainCamera.aspect;

        Vector3 camPos = mainCamera.transform.position;
        float spawnDistance = 5f; // Spawn this far away from view

        float angleStep = 360f / count; // Divide the circle around the camera into segments

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            // Calculate position on circle
            float x = camPos.x + Mathf.Cos(angle) * (cameraWidth * 2f * spawnDistance);
            float y = camPos.y + Mathf.Sin(angle) * (cameraHeight * 2f * spawnDistance);

            // Clamp y to lower boundary, otherwise they come to fast and hard for player to see
            y = Mathf.Max(y, minY);

            positions.Add(new Vector2(x, y));
        }

        return positions;
    }

    List<int> GenerateEnemyTypeList(int count)
    {
        List<int> types = new List<int>();

        // Mix of enemy types
        // 0 = basic, 1 = fast, 2 = tank
        int basicCount = Mathf.CeilToInt(count * 0.5f); // 50% basic
        int fastCount = Mathf.CeilToInt(count * 0.3f); // 30% fast
        int tankCount = count - (basicCount + fastCount); // 20% tank

        // Add types to pool
        for (int i = 0; i < basicCount; i++)
        {
            types.Add(0);
        }
        for (int i = 0; i < fastCount; i++)
        {
            types.Add(1);
        }
        for (int i = 0; i < tankCount; i++)
        {
            types.Add(2);
        }

        // Shuffle the list so they spawn in a mixture
        for (int i = 0; i < types.Count; i++)
        {
            int temp = types[i];
            int randomIndex = Random.Range(i, types.Count);
            types[i] = types[randomIndex];
            types[randomIndex] = temp;
        }

        return types;
    }

    GameObject GetEnemyFromPool(int enemyType)
    {
        Queue<GameObject> pool = null;

        switch (enemyType)
        {
            case 0: pool = basicEnemyPool;
                break;
            case 1: pool = fastEnemyPool;
                break;
            case 2: pool = tankEnemyPool;
                break;
        }

        if (pool != null && pool.Count > 0)
        {
            GameObject enemy = pool.Dequeue();

            // Reset enemy health
            var enemyScript = enemy.GetComponent<Enemy>();

            return enemy;
        }

        return null;
    }

    public void ReturnEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        activeEnemyCount--;

        // Add back to type pool
        if (enemy.CompareTag("Basic Enemy"))
        {
            basicEnemyPool.Enqueue(enemy);
        }
        else if (enemy.CompareTag("Fast Enemy"))
        {
            fastEnemyPool.Enqueue(enemy);
        }
        else if (enemy.CompareTag("Tank Enemy"))
        {
            tankEnemyPool.Enqueue(enemy);
        }
    }

    bool AreAllEnemiesDefeated()
    {
        return !isSpawningWave && activeEnemyCount <= 0;
    }
}
