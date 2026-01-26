using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPoolManager : MonoBehaviour
{
	// Prefabs to be spawned
	[SerializeField] private Transform regArrowPoolParent;
	[SerializeField] private Transform explArrowPoolParent;
	[SerializeField] private Transform enchArrowPoolParent;
	private Queue<GameObject> regArrowPool = new Queue<GameObject>();
	private Queue<GameObject> explArrowPool = new Queue<GameObject>();
	private Queue<GameObject> enchArrowPool = new Queue<GameObject>();

	[SerializeField] private GameObject bowObject;
	[SerializeField] private float speed = 10.0f;
	[SerializeField] private Vector3 offset;

	private Queue<GameObject> arrowPool = new Queue<GameObject>();
	private int poolSize = 10;

	public static ArrowPoolManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		InitializePools();
	}

	void InitializePools()
	{
		// Add enemies to the queues from the pool game objects
		foreach (Transform child in regArrowPoolParent)
		{
			child.gameObject.SetActive(false);
			regArrowPool.Enqueue(child.gameObject);
		}
		foreach (Transform child in explArrowPoolParent)
		{
			child.gameObject.SetActive(false);
			explArrowPool.Enqueue(child.gameObject);
		}
		foreach (Transform child in enchArrowPoolParent)
		{
			child.gameObject.SetActive(false);
			enchArrowPool.Enqueue(child.gameObject);
		}
	}

	public void SpawnArrow(int arrowType)
	{
		GameObject arrow = GetArrowFromPool(arrowType);

		if (arrow == null)
		{
			return;
		}

		arrow.SetActive(true);
		arrow.transform.rotation = bowObject.transform.rotation;
		arrow.transform.position = bowObject.transform.position - offset;
		arrow.GetComponent<Rigidbody2D>().velocity = arrow.transform.up * speed;

		Handheld.Vibrate();
	}

	GameObject GetArrowFromPool(int arrowType)
	{
		Queue<GameObject> pool = null;

		// If arrow type is x, then pool = xPool
		// 0 = regular, 1 = explosive, 2 = enchanted
		switch (arrowType)
		{
			case 0:
				pool = regArrowPool;
				break;
			case 1:
				pool = explArrowPool;
				break;
			case 2:
				pool = enchArrowPool;
				break;
		}

		if (pool != null && pool.Count > 0)
		{
			return pool.Dequeue();
		}

		return null;
	}

	public void ReturnArrowToPool(GameObject arrow)
	{
		// Turn off
		arrow.SetActive(false);

		// Add back to type pool
		if (arrow.CompareTag("Regular Arrow"))
		{
			regArrowPool.Enqueue(arrow);
		}
		else if (arrow.CompareTag("Explosive Arrow"))
		{
			explArrowPool.Enqueue(arrow);
		}
		else if (arrow.CompareTag("Enchanted Arrow"))
		{
			enchArrowPool.Enqueue(arrow);
		}
	}
}
