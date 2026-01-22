using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

public class TankEnemy : Enemy
{
    private List<Node> path;
    private Player player;

    private float pathUpdateInterval = 0.3f;
    private float pathUpdateTimer = 0f;
    private int lookAheadNodes = 3;

    void Start()
    {
        health = 3;
        spawnHealth = 3;
        movementSpeed = 5f;
        scoreValue = 200;

        // Find the player
        player = Player.Instance;

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

    void OnEnable()
    {
        path = null; // Clear previous path
        pathUpdateTimer = UnityEngine.Random.Range(0, pathUpdateInterval); // Stagger updates
    }

    protected override void Move()
    {
        if (path == null || path.Count == 0)
        {
            return; // No valid path
        }

        Vector3 targetPos = FindCentre(path, lookAheadNodes);

        // Only move if target is at a fair distance
        float distanceToTarget = Vector2.Distance(transform.position, targetPos);

        if(distanceToTarget > PathfindingGrid.Instance.nodeSize * 10)
        {
            targetPos = path[0].nodePos; // Move towards first node
        }

        // Smooth movement
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, movementSpeed * Time.deltaTime);
        transform.position = new Vector3(smoothPos.x, smoothPos.y, 0);

        if (Vector2.Distance(transform.position, targetPos) < PathfindingGrid.Instance.nodeSize * 2)
        {
            path.RemoveAt(0);
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