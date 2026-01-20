using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	// Enemy variables
    protected int health;
    protected int damage;
	protected int scoreValue;
    protected float movementSpeed;

	public static event System.Action<Vector2> OnEnemyDeath; // For collecting position for healing
	public static event System.Action<int> OnEnemyKilled; // For collecting score per enemy

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
				// Invoke events before destroying enemy
				OnEnemyDeath?.Invoke(transform.position);
				OnEnemyKilled?.Invoke(scoreValue);
				Destroy(gameObject);
            }
		}
	}
}