using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

static class Heuristic
{
	public static float GetDistanceEuclidean(Node nodeA, Node nodeB)
	{
		float distanceX = Mathf.Pow((nodeB.x - nodeA.x), 2);
		float distanceY = Mathf.Pow((nodeB.y - nodeA.y), 2);
		return Mathf.Sqrt(distanceX + distanceY);
	}

	public static float GetDistanceEuclideanNoSqr(Node nodeA, Node nodeB)
	{
		float distanceX = Mathf.Pow((nodeB.x - nodeA.x), 2);
		float distanceY = Mathf.Pow((nodeB.y - nodeA.y), 2);
		return (distanceX + distanceY);
	}

	public static float GetDistanceManhattan(Node nodeA, Node nodeB)
	{
		int distanceX = Mathf.Abs(nodeB.x - nodeA.x);
		int distanceY = Mathf.Abs(nodeB.y - nodeA.y);
		int cost = 1;
		return cost * (distanceX + distanceY);
	}

	public static float GetDistanceDiag(Node nodeA, Node nodeB)
	{
		int distanceX = Mathf.Abs(nodeB.x - nodeA.x);
		int distanceY = Mathf.Abs(nodeB.y - nodeA.y);
		int cost = 1;
		return cost * Mathf.Max(distanceX, distanceY);
	}
	public static float GetDistanceDiagShort(Node nodeA, Node nodeB)
	{
		int distanceX = Mathf.Abs(nodeB.x - nodeA.x);
		int distanceY = Mathf.Abs(nodeB.y - nodeA.y);
		float cost = 1.41f;

		if (distanceX > distanceY)
		{
			return (cost * distanceY) + (distanceX - distanceY);
		}
		else
		{
			return (cost * distanceX) + (distanceY - distanceX);
		}
	}
}