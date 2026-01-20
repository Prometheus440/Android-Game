using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    // If player health < 3 and an enemy has been killed
    // 15% of spawn at enemy position
    // If arrow shoots heal object, player health + 1

    private Player playerScript;
    [SerializeField] private GameObject healPrefab;
    private int spawnPercentage = 20;
    private int spawnInt;

    void Start()
    {
        playerScript = FindObjectOfType<Player>();
        Enemy.OnEnemyDeath += Spawn; // Subscribe to enemy death events
    }

    void OnDestroy()
    {
        // Unsubscribe to enemy death events
        Enemy.OnEnemyDeath -= Spawn;

    }

    void Spawn(Vector2 deathPos)
    {
        if (playerScript.health < 3)
        {
            // Inclusive min and exlusive max
            spawnInt = UnityEngine.Random.Range(1, 101);

            if (spawnInt <= spawnPercentage)
            {
                Instantiate(healPrefab, deathPos, Quaternion.identity);
            }
        }
    }
}