using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Enemy>() != null || collision.CompareTag("Heal"))
		{
			ArrowPoolManager.Instance.ReturnArrowToPool(gameObject);
		}
	}
}