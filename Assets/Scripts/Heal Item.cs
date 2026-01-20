using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
	private Player playerScript;

	// Start is called before the first frame update
	void Start()
    {
		playerScript = FindObjectOfType<Player>();
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
