using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BasicEnemy : Enemy
{
    protected override void Start()
    {
        health = 1;
        damage = 1;
        movementSpeed = 3f;

        base.Start(); // Initialise pathfinding
    }

    protected override void Move()
    {
        if (pathFound != null && pathFound.Count > 0)
        {
            Vector3 playerPos = pathFound[0];
            Vector2 direction = new Vector2(playerPos.x - transform.position.x, playerPos.y - transform.position.y).normalized;

            transform.position += new Vector3(direction.x, direction.y, 0) * movementSpeed * Time.deltaTime;

            // Remove nodes
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(playerPos.x, playerPos.y)) < 0.2f)
            {
                pathFound.RemoveAt(0);
            }
        }
    }
}
