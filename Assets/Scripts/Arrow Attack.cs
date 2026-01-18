using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			Destroy(collision.gameObject);
			Destroy(gameObject);
		}
		if (collision.gameObject.tag == "Bounds")
		{
			Destroy(gameObject);
		}
	}
}