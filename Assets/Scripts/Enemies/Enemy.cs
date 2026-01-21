using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	// Enemy variables
    protected int health;
	protected int spawnHealth;
    protected int damage;
	protected int scoreValue;
    protected float movementSpeed;

	public static event System.Action<Vector2> OnEnemyDeath; // For collecting position for healing
	public static event System.Action<int> OnEnemyKilled; // For collecting score per enemy

	private EnemyPoolManager poolManager;

    protected abstract void Move();

	public virtual void ResetEnemy()
	{
		health = spawnHealth;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		// If collision with an arrow
		if (collision.gameObject.CompareTag("Arrow"))
		{
			health--;
			Destroy(collision.gameObject);

			if (health <= 0)
			{
				// Invoke events before returning enemy to pool
				OnEnemyDeath?.Invoke(transform.position);
				OnEnemyKilled?.Invoke(scoreValue);
				
				if (poolManager != null)
				{
					poolManager.ReturnEnemyToPool(gameObject);
				}
				else
				{
					Destroy(gameObject);
				}
            }
		}
	}
}