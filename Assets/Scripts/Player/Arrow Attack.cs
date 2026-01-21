using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : MonoBehaviour
{
	private ArrowProjectile arrowProjectileScript;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Heal")
		{
			arrowProjectileScript.ReturnArrow(gameObject);
		}
	}
}