using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	// Enemy variables
    protected int health;
    protected int damage;
    protected float movementSpeed;

	// Pathfinding
	protected AStar aStarScript;
	protected List<Vector3> pathFound = new List<Vector3>();
	protected GameObject player;

	protected virtual void Start()
	{
		aStarScript = GameObject.FindObjectOfType<AStar>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	protected virtual void Update()
	{
		if (player != null && health > 0)
		{
			FindPathToPlayer();
			Move();
		}
	}

	protected void FindPathToPlayer()
	{
		if (aStarScript != null && player != null)
		{
			List<Node> path = aStarScript.RequestPath(this.gameObject, player);

			if (path != null && path.Count > 0)
			{
				pathFound.Clear();

				foreach (Node node in path)
				{
					pathFound.Add(node.nodePos);
				}
			}
		}
	}

	protected abstract void Move();

	protected virtual void OnCollisionEnter2D(Collision2D collision)
	{
		if (health > 0)
		{
			if (collision.gameObject.tag == "Arrow")
			{
				health--;
			}
		}
		else if (health <= 0)
		{
			Destroy(gameObject);
		}
	}

	protected Vector3 GetNextPathPosition()
	{
		if (pathFound != null && pathFound.Count > 0)
		{
			return pathFound[0];
		}

		return transform.position;
	}
}