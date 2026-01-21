using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
	private Player playerScript;

	void Start()
	{
		playerScript = Player.Instance;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		// If collision with an arrow
		if (collision.gameObject.CompareTag("Arrow"))
		{
			playerScript.Heal(1);
			Destroy(collision.gameObject);
			Destroy(gameObject);
		}
	}
}