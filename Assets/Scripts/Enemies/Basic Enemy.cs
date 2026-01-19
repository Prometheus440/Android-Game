using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

public class BasicEnemy : Enemy
{
    private List<Node> path;
    private GameObject player;

    private float pathUpdateInterval = 0.3f;
    private float pathUpdateTimer = 0f;
    private int lookAheadNodes = 3;

    void Start()
    {
        health = 2;
        movementSpeed = 15f;

        // Find the player
        player = GameObject.FindGameObjectWithTag("Player");

        // So enemies don't update their path at the same time
        pathUpdateTimer = UnityEngine.Random.Range(0f, pathUpdateInterval);
    }

    private void Update()
    {
        if (player != null && PathfindingGrid.Instance != null)
        {
            // Start timer
            pathUpdateTimer += Time.deltaTime;

            // If timer has reached 0.5f
            if (pathUpdateTimer >= pathUpdateInterval)
            {
                // Check path
                path = PathfindingGrid.Instance.RequestPath(transform.position, player.transform.position);
                // Restart timer
                pathUpdateTimer = 0f;
            }

            Move();
        }
    }

    protected override void Move()
    {
        if (path != null && path.Count > 0)
        {
			Vector3 targetPos = FindCentre(path, lookAheadNodes);

            // Smooth movement
            Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, movementSpeed * Time.deltaTime);
            transform.position = new Vector3(smoothPos.x, smoothPos.y, 0);

			if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(targetPos.x, targetPos.y)) < PathfindingGrid.Instance.nodeSize * 2)
			{
				path.RemoveAt(0);
			}
		}
    }

    Vector3 FindCentre(List<Node> _path, int nodesToAverage)
    {
        int nodesToCheck = Mathf.Min(nodesToAverage, _path.Count);

        float x = 0;
        float y = 0;

        for (int i = 0; i < nodesToCheck; i++)
        {
            x += _path[i].nodePos.x;
            y += _path[i].nodePos.y;
        }

        x /= nodesToCheck;
        y /= nodesToCheck;

        return new Vector3(x, y, 0);
    }
}