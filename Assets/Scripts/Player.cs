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
		//Getting the script and sprite
        sr_bowSprite = GetComponent<SpriteRenderer>();
		script_UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();

		//Enable's gyro and stores inputs in m_gyro
		m_Gyro = Input.gyro; 
		Input.gyro.enabled=true;

		//Game objects to cycle between arrows shown
		regularArrow = GameObject.Find("Regular Arrow");
		explosiveArrow = GameObject.Find("Explosive Arrow");
		enchantedArrow = GameObject.Find("Enchanted Arrow");

		//Game objects used to switch the position of arrows shown
		middleArrow = GameObject.Find("Middle Arrow").GetComponent<Transform>();
		leftArrow = GameObject.Find("Left Arrow").GetComponent<Transform>();
		rightArrow = GameObject.Find("Right Arrow").GetComponent<Transform>();
    }

    void Update()
    {
		//Gets values needed to check if the player has tapped or swiped on the screen
		if(Input.touchCount == 1)
		{
			if(Input.touches[0].phase == TouchPhase.Began)
			{
				fingerTouchDown = Input.touches[0].position;
			}
			if(Input.touches[0].phase == TouchPhase.Ended)
			{
				fingerTouchUp = Input.touches[0].position;
				CheckSipe();//Fires bow or changes arrow type based on input
			}
		}
		GyroInput();//Rotatates player based on device tilt
	}

	//Checks if player has swiped
	void CheckSipe()
	{
		//Checks the swipe value to determine direction or if pressed
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
		//Checks which arrow is currently equiped and changes it respectively
		if(regularArrow.transform.position == middleArrow.position)
		{
			ArrowPositionLeft1();
			return;
		}
		if(regularArrow.transform.position == leftArrow.position)
		{
			ArrowPositionLeft2();
			return;
		}
		if(regularArrow.transform.position == rightArrow.position)
		{
			ArrowPositionLeft3();
			return;
		}
	}

	void SwipeRight()
	{
		//Checks which arrow is currently equiped and changes it respectively
		if(regularArrow.transform.position == middleArrow.position)
		{
			ArrowPositionRight1();
			return;
		}
		if(regularArrow.transform.position == rightArrow.position)
		{
			ArrowPositionRight2();
			return;
		}
		if(regularArrow.transform.position == leftArrow.position)
		{
			ArrowPositionRight3();
			return;
		}
	}

	//Regular arrow in middle, moves all right
	void ArrowPositionRight1()
	{
		explosiveArrow.transform.position = middleArrow.transform.position;
		regularArrow.transform.position = rightArrow.transform.position;
		enchantedArrow.transform.position = leftArrow.transform.position;
	}
	//Regular arrow on right, moves all right
	void ArrowPositionRight2()
	{
		explosiveArrow.transform.position = rightArrow.transform.position;
		regularArrow.transform.position = leftArrow.transform.position;
		enchantedArrow.transform.position = middleArrow.transform.position;
	}
	//Regular arrow on left, moves all right
	void ArrowPositionRight3()
	{
		explosiveArrow.transform.position = leftArrow.transform.position;
		regularArrow.transform.position = middleArrow.transform.position;
		enchantedArrow.transform.position = rightArrow.transform.position;
	}
	//Regular arrow in middle, moves all left
	void ArrowPositionLeft1()
	{
		explosiveArrow.transform.position = rightArrow.transform.position;
		regularArrow.transform.position = leftArrow.transform.position;
		enchantedArrow.transform.position = middleArrow.transform.position;
	}
	//Regular arrow on left, moves all left
	void ArrowPositionLeft2()
	{
		explosiveArrow.transform.position = middleArrow.transform.position;
		regularArrow.transform.position = rightArrow.transform.position;
		enchantedArrow.transform.position = leftArrow.transform.position;
	}
	//Regular arrow on right, moves all left
	void ArrowPositionLeft3()
	{
		explosiveArrow.transform.position = leftArrow.transform.position;
		regularArrow.transform.position = middleArrow.transform.position;
		enchantedArrow.transform.position = rightArrow.transform.position;
	}

	void GyroInput()
    {
		transform.rotation = GyroToUnity(Input.gyro.attitude);
    }
	
	//Returns 
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