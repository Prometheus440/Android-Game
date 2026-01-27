using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveArrow : MonoBehaviour
{
	[SerializeField] GameObject explosionAOEPrefab;
	private float duration = 0.5f;

    private void Update()
    {
		// Return arrow to pool if it goes too far off screen
		if (transform.position.y > Camera.main.transform.position.y + 15f ||
			transform.position.y < Camera.main.transform.position.y - 15f)
		{
			ArrowPoolManager.Instance.ReturnArrowToPool(gameObject);
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Enemy>() != null || collision.CompareTag("Heal"))
		{
			Explode();
		}
	}

	void Explode()
	{
		GameObject explosion = Instantiate(explosionAOEPrefab, transform.position, Quaternion.identity);
		ArrowPoolManager.Instance.ReturnArrowToPool(gameObject);
		Destroy(explosion, duration);
	}
}