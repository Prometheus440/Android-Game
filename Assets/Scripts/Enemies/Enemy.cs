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

	private SpriteRenderer enemySprite;

	public static event System.Action<Vector2> OnEnemyDeath; // For collecting position for healing
	public static event System.Action<int> OnEnemyKilled; // For collecting score per enemy

	[SerializeField] private AudioClip arrowHitAudio;
	[SerializeField] private AudioClip deathAudio;
	private AudioSource audioSource;

	protected EnemyPoolManager poolManager;

    private void Awake()
    {
		audioSource = GetComponent<AudioSource>();
		enemySprite = GetComponent<SpriteRenderer>();
    }

    public void SetPoolManager(EnemyPoolManager manager)
	{
		poolManager = manager;
	}

    protected abstract void Move();

	public virtual void ResetEnemy()
	{
		health = spawnHealth;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		// If collision with an arrow
		if (collision.gameObject.CompareTag("Regular Arrow") || collision.gameObject.CompareTag("Explosive Arrow") || collision.gameObject.CompareTag("Enchanted Arrow") || collision.gameObject.CompareTag("Explosion"))
		{
			if (collision.gameObject.CompareTag("Regular Arrow") || collision.gameObject.CompareTag("Explosive Arrow"))
			{
				ArrowPoolManager.Instance.ReturnArrowToPool(collision.gameObject);
			}

			health--;

			if (health > 0)
			{
				// audioSource.PlayOneShot(arrowHitAudio); // Sound effect
				StartCoroutine(FlashRed());
			}
			else if (health <= 0)
			{
				// Invoke events before returning enemy to pool
				OnEnemyDeath?.Invoke(transform.position);
				OnEnemyKilled?.Invoke(scoreValue);

				audioSource.PlayOneShot(deathAudio); // Sound effect

				poolManager.ReturnEnemyToPool(gameObject);
            }
		}
	}

	// Flashing red after damage
	private IEnumerator FlashRed()
	{
		for (int i = 0; i < 2; i++)
		{
			enemySprite.color = Color.red;
			yield return new WaitForSeconds(0.1f);
			enemySprite.color = Color.white;
			yield return new WaitForSeconds(0.1f);
		}
	}
}