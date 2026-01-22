using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
	[SerializeField] private GameObject arrowPrefab;
	[SerializeField] private GameObject bowObject;
	[SerializeField] private float speed = 10.0f;
	[SerializeField] private Vector3 offset;

	private Queue<GameObject> arrowPool = new Queue<GameObject>();
	private int poolSize = 10;

	void Start()
	{
		// Create pool at start
		for (int i = 0; i < poolSize; i++)
		{
			GameObject arrow = Instantiate(arrowPrefab);
			arrow.SetActive(false);
			arrowPool.Enqueue(arrow);
		}
	}

	public void SpawnArrow()
	{
		GameObject shot;

		if (arrowPool.Count > 0)
		{
			shot = arrowPool.Dequeue();
			shot.SetActive(true);
		}
		else
		{
			shot = Instantiate(arrowPrefab); // Only do this if pool is empty
		}

		Handheld.Vibrate();
		shot.transform.rotation = bowObject.transform.rotation;
		shot.transform.position = bowObject.transform.position - offset;
		shot.GetComponent<Rigidbody2D>().velocity = shot.transform.up * speed;
	}

	public void ReturnArrow(GameObject arrow)
	{
		arrow.SetActive(false);
		arrowPool.Enqueue(arrow);
	}
}