using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	// Enemy variables
    protected int health;
    protected int damage;
    protected float movementSpeed;

	public static event System.Action<Vector2> OnEnemyDeath;

    protected abstract void Move();

	void OnCollisionEnter2D(Collision2D collision)
	{
		// If collision with an arrow
		if (collision.gameObject.CompareTag("Arrow"))
		{
			health--;
			Destroy(collision.gameObject);

			if (health <= 0)
			{
				// Invoke event before destroying enemy
				OnEnemyDeath?.Invoke(transform.position);
				Destroy(gameObject);
            }
		}
	}
}