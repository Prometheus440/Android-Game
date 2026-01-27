using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private SpriteRenderer bowSprite;
	[SerializeField] private UIManager UIManagerScript;
	private Animator fireAnimation;
	public int health = 6;
	private bool canFire = true;
	[SerializeField] private AudioClip arrowReleaseAudio;

	// Input modes
	private enum InputMode { Touch, Accel, Swipe }
	private InputMode inpMode = InputMode.Touch;
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

    // Animation times
    [SerializeField] private float arrowReleaseTime = 0.4f;
	[SerializeField] private float animationDuration = 0.83f;
	private bool isFiring;

	public static Player Instance;

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
        //Getting the scripts and sprite
		bowSprite = GetComponent<SpriteRenderer>();
		UIManagerScript.UpdateLives(health);
		fireAnimation = GetComponent<Animator>();

        //Enable's gyro and stores inputs in m_gyro
        m_Gyro = Input.gyro;
        Input.gyro.enabled = true;

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
        if (Input.touchCount == 1)
        {
            Debug.Log("Test");
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                fingerTouchDown = Input.touches[0].position;
            }
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                fingerTouchUp = Input.touches[0].position;
                CheckSwipe();//Fires bow or changes arrow type based on input
            }
        }
        GyroInput();//Rotatates player based on device tilt
    }

    void CheckSwipe()
    {
        //Checks the swipe value to determine direction or if pressed
        if (fingerTouchDown.x - fingerTouchUp.x < -100)
        {
            SwipeRight();
        }
        if (fingerTouchDown.x - fingerTouchUp.x > 100)
        {
            SwipeLeft();
        }
        if (fingerTouchDown.x - fingerTouchUp.x > -100 && fingerTouchDown.x - fingerTouchUp.x < 100)
        {
            Shoot();
        }
    }
    void SwipeLeft()
    {
        //Checks which arrow is currently equiped and changes it respectively
        if (regularArrow.transform.position == middleArrow.position)
        {
            ArrowPositionLeft1();
            return;
        }
        if (regularArrow.transform.position == leftArrow.position)
        {
            ArrowPositionLeft2();
            return;
        }
        if (regularArrow.transform.position == rightArrow.position)
        {
            ArrowPositionLeft3();
            return;
        }
    }

    void SwipeRight()
    {
        //Checks which arrow is currently equiped and changes it respectively
        if (regularArrow.transform.position == middleArrow.position)
        {
            ArrowPositionRight1();
            return;
        }
        if (regularArrow.transform.position == rightArrow.position)
        {
            ArrowPositionRight2();
            return;
        }
        if (regularArrow.transform.position == leftArrow.position)
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
        return new Quaternion(0, 0, -q.y, q.w);

    }

    private void Shoot() //Shoots the bow
    {
        if (Input.touchCount > 0 && !isFiring && ArrowPoolManager.Instance.CanFire()) // When screen pressed
        {
            fireAnimation = GetComponent<Animator>();
            fireAnimation.SetTrigger("Fire"); // Plays firing animation
            StartCoroutine(FireSequence());
        }
    }

    private IEnumerator FireSequence()
	{
        // Check to see if it can fire before anything else
        if (!ArrowPoolManager.Instance.CanFire())
        {
            yield break;
        }

		canFire = false;
		isFiring = true;

		fireAnimation.Rebind();
		fireAnimation.Update(0f);
		fireAnimation.Play("EnchantedBowFire", 0, 0f); // Plays firing animation
        AudioSource.PlayClipAtPoint(arrowReleaseAudio, transform.position, 1f); // Sound effect

		// Wait to spawn arrow prefab
		yield return new WaitForSeconds(arrowReleaseTime);
        int arrowType = GetCurrentArrowType();
        ArrowPoolManager.Instance.SpawnArrow(arrowType);

		// Wait to be able to fire again
		yield return new WaitUntil(() => fireAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !fireAnimation.IsInTransition(0));
		canFire = true;
		isFiring = false;
	}

	public void Heal(int amount)
	{
		if (health < 6)
		{
			health = Mathf.Min(health + amount, 6); // Max health cap
			UIManagerScript.UpdateLives(health);
		}
	}

	public void TakeDamage(int damage)
	{
		// Damage
		if (health > 0)
		{
			health -= damage;
			StartCoroutine(FlashRed());
			UIManagerScript.UpdateLives(health);
		}
		// Death
		if (health <= 0)
		{
			Die();
		}
	}

	public void Die()
	{
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// If collision with an enemy
		GameObject enemy = collision.gameObject;

		// Amounts of damage per enemy type
		if (collision.gameObject.CompareTag("Basic Enemy"))
		{
			TakeDamage(2);
			EnemyPoolManager.Instance.ReturnEnemyToPool(enemy);
		}
		else if (collision.gameObject.CompareTag("Fast Enemy"))
		{
			TakeDamage(1);
			EnemyPoolManager.Instance.ReturnEnemyToPool(enemy);
		}
		else if (collision.gameObject.CompareTag("Tank Enemy"))
		{
			TakeDamage(4);
			EnemyPoolManager.Instance.ReturnEnemyToPool(enemy);
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

    public int GetHealth()
    {
        return health;
    }
   
    public void ResetHealth()
    {
        health = 6;
        UIManagerScript.UpdateLives(health);
    }

    int GetCurrentArrowType()
    {
        if (regularArrow.transform.position == middleArrow.position)
        {
            return 0;
        }
        else if (explosiveArrow.transform.position == middleArrow.position)
        {
            return 1;
        }
        else if (enchantedArrow.transform.position == middleArrow.position)
        {
            return 2;
        }

        return 0; // Default
    }
}