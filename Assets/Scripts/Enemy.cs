using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private float movementSpeed = 5f;

	// Pathfinding variables
	Node[,] grid;
	Vector2 gridSize;
	Vector2 gridNodes;
	Vector3 rootNodePos, goalNodePos;
	LayerMask obstacleLayer;
	float nodeSize = 0.1f;
	List<Node> openSet;
	List<Node> closedSet;
	List<Node> path;
	bool pathFound;
	bool searching = false;

	void Start()
	{
		gridSize = transform.localScale * 10f;
		gridNodes = new Vector2(Mathf.RoundToInt(gridSize.x / nodeSize), Mathf.RoundToInt(gridSize.y / nodeSize));
		obstacleLayer = LayerMask.GetMask("Obstacle");
		//CreateGrid();
	}

	void Update()
	{
		transform.Translate(0, -movementSpeed * Time.deltaTime, 0);
	}

	/* 
    ==================================
             A* PATHFINDING
    ==================================
    */
}