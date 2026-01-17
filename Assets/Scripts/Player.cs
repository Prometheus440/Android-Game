using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private SpriteRenderer bowSprite;
	private UIManager UIManagerScript;
	private ArrowProjectile arrowProjectileScript;
	private Animator fireAnimation;
	private int health = 3;
	private float rotationSpeed = 150.0f;
	private bool canFire = true;
	private enum InputMode { Touch, Accel, Swipe }
	private InputMode inpMode = InputMode.Touch;
	Vector2 fingerDown;
	Vector2 fingerUp;
	Gyroscope m_Gyro;
	Vector3 rot;

	// Animation times
	[SerializeField] private float arrowReleaseTime = 0.4f;
	[SerializeField] private float animationDuration = 0.83f;
	private bool isFiring;

	void Start()
	{
		bowSprite = GetComponent<SpriteRenderer>();
		UIManagerScript = GameObject.Find("Canvas").GetComponent<UIManager>();
		arrowProjectileScript = GetComponent<ArrowProjectile>();
		fireAnimation = GetComponent<Animator>();
		m_Gyro = Input.gyro;
		Input.gyro.enabled = true;
	}

	void Update()
	{
		if (inpMode == InputMode.Touch)
		{
			Shoot();
		}

		if (!isFiring)
		{
			TestGyro();
		}
	}

	void TestGyro()
	{
		Quaternion quat = Quaternion.Euler(rot);
		//Output the rotation rate, attitude and the enabled state of the gyroscope as a Label
		Debug.Log("Gyro attitude" + m_Gyro.attitude);

		quat = GyroToUnity(Input.gyro.attitude);
		transform.rotation = quat;
	}

	private static Quaternion GyroToUnity(Quaternion q)
	{
		return new Quaternion(0, 0, -q.y, q.w);
	}

	private void ArrowType()
	{
		print("Swipe");
	}


	private void Shoot() //Shoots the bow
	{
		// PC TESTING
		if (Input.GetMouseButtonDown(0) && canFire)
		{
			StartCoroutine(FireSequence());
		}

		if (Input.touchCount > 0 && canFire)// When screen pressed
		{
			StartCoroutine(FireSequence());
		}
	}

	private IEnumerator FireSequence()
	{
		canFire = false;
		isFiring = true;

		fireAnimation.Rebind();
		fireAnimation.Update(0f);
		fireAnimation.Play("EnchantedBowFire", 0, 0f); // Plays firing animation

		// Wait to spawn arrow prefab
		yield return new WaitForSeconds(arrowReleaseTime);
		arrowProjectileScript.SpawnArrow();

		// Wait to be able to fire again
		yield return new WaitUntil(() => fireAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !fireAnimation.IsInTransition(0));
		canFire = true;
		isFiring = false;
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		// If collision with an enemy
		if (collision.gameObject.CompareTag("Enemy"))
		{
			// Damage
			if (health > 0)
			{
				health--;
				StartCoroutine(FlashRed());
				UIManagerScript.UpdateLives(health);
			}

			// Death
			if (health <= 0)
			{
				Destroy(gameObject);
			}
		}
	}

	// Flashing red after damage
	private IEnumerator FlashRed()
	{
		for (int i = 0; i < 4; i++)
		{
			bowSprite.color = Color.red;
			yield return new WaitForSeconds(0.1f);
			bowSprite.color = Color.white;
			yield return new WaitForSeconds(0.1f);
		}
	}
}