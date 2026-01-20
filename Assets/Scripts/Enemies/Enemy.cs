using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	// Enemy variables
    protected int health;
    protected int damage;
    protected float movementSpeed;

	public bool enemyKilled = false;
	public Vector2 deathPos;

    protected abstract void Move();

	void OnCollisionEnter2D(Collision2D collision)
	{
		// If collision with an enemy
		if (collision.gameObject.CompareTag("Arrow"))
		{
			health--;
			Destroy(collision.gameObject);

			if (health <= 0)
			{
				Destroy(gameObject);
				enemyKilled = true;
				deathPos = transform.position;
            }
		}
	}
}