using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class fastEnemy : Enemy
{
	private List<Node> path;

	private GameObject player;
	private float pathUpdateInterval = 0.3f;
	private float pathUpdateTimer = 0f;

	void Start()
	{
		health = 1;
		movementSpeed = 5f;

		// Find the player
		player = GameObject.FindGameObjectWithTag("Player");
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
			Vector3 targetPos = path[0].nodePos;
			Vector2 direction = new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y).normalized;

			// Move
			transform.position += new Vector3(direction.x, direction.y, 0) * movementSpeed * Time.deltaTime;

			if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(targetPos.x, targetPos.y)) < PathfindingGrid.Instance.nodeSize * 2)
			{
				path.RemoveAt(0);
			}
		}
	}
}