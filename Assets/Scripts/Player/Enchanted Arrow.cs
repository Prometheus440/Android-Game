using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantedArrow : MonoBehaviour
{
	private int pierceNumber = 3;
	private int initPierceNumber = 3;
	private HashSet<Enemy> hitEnemies = new HashSet<Enemy>();
	private Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		rb.velocity = rb.velocity.normalized * rb.velocity.magnitude;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Enemy enemy = collision.GetComponentInParent<Enemy>();

		if (enemy != null)
		{
			if (hitEnemies.Contains(enemy))
				return;

			hitEnemies.Add(enemy);
			pierceNumber--;
			Debug.Log($"Pierced enemy. Remaining: {pierceNumber}");

			if (pierceNumber <= 0)
			{
				ArrowPoolManager.Instance.ReturnArrowToPool(gameObject);
			}
		}
	}
}