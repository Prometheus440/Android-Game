using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
	private Player playerScript;
	private GameManager gameManager;

	void Start()
	{
		playerScript = Player.Instance;
		gameManager = FindObjectOfType<GameManager>();
	}

	private void Update()
	{
		Moving();
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		// If collision with an arrow
		if (collision.gameObject.CompareTag("Regular Arrow") || collision.gameObject.CompareTag("Explosive Arrow") || collision.gameObject.CompareTag("Enchanted Arrow"))
		{
			// Heal player half a heart
			playerScript.Heal(1);
			Destroy(collision.gameObject);
			Destroy(gameObject);
		}
	}

	void Moving()
	{
		if (gameManager.isScrolling)
		{
			// Move prefab down at the same rate as the screen
			transform.position += Vector3.down * gameManager.scrollSpeed * Time.deltaTime;
		}
	}
}