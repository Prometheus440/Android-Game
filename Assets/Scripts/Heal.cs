using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    // If player health < 3 and an enemy has been killed
    // 15% of spawn at enemy position
    // If arrow shoots heal object, player health + 1

    private Player playerScript;
    private Enemy enemyScript;
    private int spawnPercentage = 100;
    private int spawnInt;

    void Start()
    {
        playerScript = GetComponent<Player>();
        enemyScript = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.health < 3 && enemyScript.enemyKilled == true)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        // Inclusive min and exlusive max
        spawnInt = UnityEngine.Random.Range(1, 101);

        if (spawnInt <= spawnPercentage)
        {
            Instantiate(this, enemyScript.deathPos, Quaternion.identity);
        }
    }
}