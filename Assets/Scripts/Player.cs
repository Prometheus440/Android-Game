using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
	Vector2 fingerTouchDown;
	Vector2 fingerTouchUp;
	GameObject regularArrow;
	GameObject explosiveArrow;
	GameObject enchantedArrow;
	Transform middleArrow;
	Transform leftArrow;
	Transform rightArrow;

    void Start()
    {
        sr_bowSprite = GetComponent<SpriteRenderer>();
		script_UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
		m_Gyro = Input.gyro;
		Input.gyro.enabled=true;
		regularArrow = GameObject.Find("Regular Arrow");
		explosiveArrow = GameObject.Find("Explosive Arrow");
		enchantedArrow = GameObject.Find("Enchanted Arrow");
		middleArrow = GameObject.Find("Middle Arrow").GetComponent<Transform>();
		leftArrow = GameObject.Find("Left Arrow").GetComponent<Transform>();
		rightArrow = GameObject.Find("Right Arrow").GetComponent<Transform>();
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

		if(Input.touchCount == 1)
		{
			if(Input.touches[0].phase == TouchPhase.Began)
			{
				fingerTouchDown = Input.touches[0].position;
			}
			if(Input.touches[0].phase == TouchPhase.Ended)
			{
				fingerTouchUp = Input.touches[0].position;
				CheckSipe();
			}
		}
		
		TestGyro();
	}
	void CheckSipe()
	{
		if(fingerTouchDown.x - fingerTouchUp.x < -100)
		{
			SwipeRight();
		}
		if(fingerTouchDown.x - fingerTouchUp.x > 100)
		{
			SwipeLeft();
		}
		if(fingerTouchDown.x - fingerTouchUp.x > -100 && fingerTouchDown.x - fingerTouchUp.x < 100)
		{
			Shoot();
		}
	}
	void SwipeLeft()
	{
		Debug.Log("Left");
	}

	void SwipeRight()
	{
			if(regularArrow.transform.position == middleArrow.position)
			{
				Debug.Log("Test1");
				ArrowPosition1();
				return;
			}
			if(regularArrow.transform.position == rightArrow.position)
			{
				Debug.Log("Test2");
				ArrowPosition2();
				return;
			}
			if(regularArrow.transform.position == leftArrow.position)
			{
				Debug.Log("Test3");
				ArrowPosition3();
				return;
			}
			
		
	
		
		
	}

	void ArrowPosition1()
	{
		explosiveArrow.transform.position = middleArrow.transform.position;
		regularArrow.transform.position = rightArrow.transform.position;
		enchantedArrow.transform.position = leftArrow.transform.position;
	}
	void ArrowPosition2()
	{
		explosiveArrow.transform.position = rightArrow.transform.position;
		regularArrow.transform.position = leftArrow.transform.position;
		enchantedArrow.transform.position = middleArrow.transform.position;
	}
	void ArrowPosition3()
	{
		explosiveArrow.transform.position = leftArrow.transform.position;
		regularArrow.transform.position = middleArrow.transform.position;
		enchantedArrow.transform.position = rightArrow.transform.position;
	}

	void TestGyro()
    {
		Quaternion quat = Quaternion.Euler(rot);

		quat = GyroToUnity(Input.gyro.attitude) ;
		transform.rotation = quat;
    }
	
	private static Quaternion GyroToUnity(Quaternion q)
	{
		return new Quaternion(0,0,-q.y,q.w);
		
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