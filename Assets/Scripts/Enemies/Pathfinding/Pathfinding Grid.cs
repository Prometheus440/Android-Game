using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    public static PathfindingGrid Instance;

	Node[,] grid;
	Vector2 gridSize;
	LayerMask obstacleLayer;
	public float nodeSize = 0.1f;
	Vector2 gridNodes;
	Heap<Node> openSet;
	HashSet<Node> closedSet;
	Node currentNode;
	List<Node> path;
	bool pathFound;
	bool searching = false;
	Vector3 rootNodePos, goalNodePos;

	void Awake()
    {
		Instance = this;	
    }

	void Start()
	{
		gridSize = transform.localScale * 10f;
		gridNodes = new Vector2(Mathf.RoundToInt(gridSize.x / nodeSize), Mathf.RoundToInt(gridSize.y / nodeSize));
		obstacleLayer = LayerMask.GetMask("Environment");
		CreateGrid();
	}

	void CreateGrid()
	{
		grid = new Node[(int)gridNodes.x, (int)gridNodes.y];
		Vector3 gridBottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.up * gridSize.y / 2;

		for (int i = 0; i < gridNodes.x; i++)
		{
			for (int j = 0; j < gridNodes.y; j++)
			{
				Vector3 nodePos = gridBottomLeft + Vector3.right * (i * nodeSize + (nodeSize / 2)) + Vector3.up * (j * nodeSize + (nodeSize / 2));
				bool traversable = !(Physics2D.OverlapCircle(new Vector2(nodePos.x, nodePos.y), nodeSize / 2, obstacleLayer));
				grid[i, j] = new Node(nodePos, traversable, i, j);
			}
		}
	}

	public List<Node> RequestPath(Vector3 startPos, Vector3 targetPos)
	{
		rootNodePos = startPos;
		goalNodePos = targetPos;
		AStarPathFind();
		return path;
	}

	public Node NodePositionInGrid(Vector3 gridPosition)
	{
		Vector3 relativePos = gridPosition - transform.position;

		float pX = Mathf.Clamp01((relativePos.x + gridSize.x / 2) / gridSize.x);
		float pY = Mathf.Clamp01((relativePos.y + gridSize.y / 2) / gridSize.y);

		int nX = (int)Mathf.Clamp(Mathf.RoundToInt(gridNodes.x * pX), 0, gridNodes.x - 1);
		int nY = (int)Mathf.Clamp(Mathf.RoundToInt(gridNodes.y * pY), 0, gridNodes.y - 1);

		return grid[nX, nY];
	}

	void AStarPathFind()
	{
		Node rootNode = NodePositionInGrid(rootNodePos);
		Node goalNode = NodePositionInGrid(goalNodePos);

		openSet = new Heap<Node>(grid.Length);
		closedSet = new HashSet<Node>();

		pathFound = false;
		searching = true;

		openSet.Add(rootNode);
		float newMoveCost;

		while (openSet.Count > 0 && searching)
		{
			currentNode = openSet.RemoveTop();
			closedSet.Add(currentNode);

			if (currentNode == goalNode)
			{
				RetracePath(rootNode, goalNode);
				pathFound = true;
				searching = false;
				break;
			}

			foreach (Node neighbour in GetNeighbours(currentNode))
			{
				if (!neighbour.traversable || closedSet.Contains(neighbour))
				{
					continue;
				}

				newMoveCost = currentNode.g + GetDistance(currentNode, neighbour);

				if (newMoveCost < neighbour.g || !openSet.Contains(neighbour))
				{
					neighbour.g = newMoveCost;
					neighbour.h = GetDistance(neighbour, goalNode);

					neighbour.parentNode = currentNode;

					if (!openSet.Contains(neighbour))
					{
						openSet.Add(neighbour);
					}
				}
			}
		}

		searching = false;
	}

	void RetracePath(Node rNode, Node gNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = gNode;

		while (currentNode != rNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parentNode;
		}

		path.Reverse();
		this.path = path;
	}

	public List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();

		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (i == 0 && j == 0)
				{
					continue;
				}

				int pX = node.x + i;
				int pY = node.y + j;

				if (pX >= 0 && pX < gridNodes.x && pY >= 0 && pY < gridNodes.y)
				{
					neighbours.Add(grid[pX, pY]);
				}
			}
		}
		return neighbours;
	}

	public float GetDistance(Node nodeA, Node nodeB)
	{
		float rValue = 0;
		rValue = Heuristic.GetDistanceEuclidean(nodeA, nodeB);
		return rValue;
	}
}