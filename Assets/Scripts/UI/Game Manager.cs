using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Transform background1;
	[SerializeField] private Transform background2;

	[SerializeField] private UIManager uiManager;
	[SerializeField] private SceneManagerScript sceneManager;

	public float scrollSpeed = 4f;
	private float scrollDuration = 5f;
	private float backgroundX = -2.148794f;
	private float backgroundY = 1.064829f;
	private float backgroundHeight = 23.87f; // Height of one background sprite

	[SerializeField] private EnemyPoolManager enemyPoolManager;

	public bool isScrolling = false;
	public bool gameOver;

	void Start()
	{
		// Set backgrounds at the start
		background1.position = new Vector3(backgroundX, backgroundY, 0);
		background2.position = new Vector3(backgroundX, backgroundY + backgroundHeight, 0);
	}

	private void Update()
	{
		StartCoroutine(CheckWaveCompletion());

		// Call scroll once per frame so in update()
		if (isScrolling && !gameOver)
		{
			ScrollBackgrounds();
		}

		GameOver();
	}

	IEnumerator CheckWaveCompletion()
	{
		while (!gameOver) // while playing
		{
			if (enemyPoolManager.AreAllEnemiesDefeated()) // If all enemies in the wave are dead
			{
				yield return StartCoroutine(ScrollSequence()); // Start scrolling

				// After scroll has finished, spawn next wave
				enemyPoolManager.SpawnWave();
			}

			yield return null;
		}
	}

	IEnumerator ScrollSequence()
	{
		isScrolling = true;
		float elapsedTime = 0f;

		// Scroll for duration
		while (elapsedTime < scrollDuration)
		{
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		// Stop scrollings
		isScrolling = false;
	}

	void ScrollBackgrounds()
	{
		// Move both backgrounds down
		background1.position += Vector3.down * scrollSpeed * Time.deltaTime;
		background2.position += Vector3.down * scrollSpeed * Time.deltaTime;

		// Loop backgrounds
		if (background1.position.y <= -backgroundHeight)
		{
			background1.position = new Vector3(backgroundX, background2.position.y + backgroundHeight, 0);
		}
		if (background2.position.y <= -backgroundHeight)
		{
			background2.position = new Vector3(backgroundX, background1.position.y + backgroundHeight, 0);
		}
	}

	void GameOver()
	{
		if (Player.Instance.GetHealth() <= 0)
		{
			gameOver = true;
			isScrolling = false;
		}
	}

	void Replay()
	{
		sceneManager.ChangeScene("Play");
    }
}