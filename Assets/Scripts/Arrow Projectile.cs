using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
	[SerializeField] private GameObject arrowObject;
	[SerializeField] private GameObject bowObject;

	private GameObject shot;
	private Rigidbody2D rb;

	[SerializeField] private float speed = 10.0f;
	[SerializeField] private Vector3 offset;

	public void SpawnArrow()
	{
		shot = Instantiate(arrowObject);
		rb = shot.GetComponent<Rigidbody2D>();
		shot.transform.rotation = bowObject.transform.rotation;
		shot.transform.position = bowObject.transform.position - offset;
		rb.velocity = shot.transform.up * speed;
	}
}