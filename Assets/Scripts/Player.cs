using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Placeholders for phone input
    // =======================================================
    private KeyCode KeyLeft = KeyCode.LeftArrow;
    private KeyCode KeyRight = KeyCode.RightArrow;
    private KeyCode KeyFire = KeyCode.UpArrow;
    // =======================================================

    private SpriteRenderer sr_bowSprite;
    private UIManager script_UIManager;
	private Animator fireAnimation;
    private int health = 3;
    private float rotationSpeed = 150.0f;
	private enum InputMode{Touch,Accel,Swipe}
	private InputMode inpMode=InputMode.Touch;
	Vector2 fingerDown;
	Vector2 fingerUp;
	Gyroscope m_Gyro;
	Vector3 rot;

    void Start()
    {
        sr_bowSprite = GetComponent<SpriteRenderer>();
		script_UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
		m_Gyro = Input.gyro;
		Input.gyro.enabled=true;
    }

    void Update()
    {
		// Rotation
		if (Input.GetKey(KeyLeft))
		{
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyRight))
		{
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
		}

		if(inpMode==InputMode.Touch)
		{
			Shoot();
		}
		TestGyro();
	}

	void TestGyro()
    {
		Quaternion quat = Quaternion.Euler(rot);
        //Output the rotation rate, attitude and the enabled state of the gyroscope as a Label
        Debug.Log("Gyro attitude" + m_Gyro.attitude);

		quat = GyroToUnity(Input.gyro.attitude) ;
		transform.rotation = quat;
    }
	
	private static Quaternion GyroToUnity(Quaternion q)
	{
		return new Quaternion(0,0,-q.y,q.w);
		
	}

	private void ArrowType()
	{
		print("Swipe");
	}


	private void Shoot() //Shoots the bow
	{
		if(Input.touchCount>0)//When screen pressed
		{
		fireAnimation = GetComponent<Animator>();
		fireAnimation.SetTrigger("Fire"); //Plays firing animation
		}
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
				script_UIManager.UpdateLives(health);
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
			sr_bowSprite.color = Color.red;
			yield return new WaitForSeconds(0.1f);
			sr_bowSprite.color = Color.white;
			yield return new WaitForSeconds(0.1f);
		}
	}
}