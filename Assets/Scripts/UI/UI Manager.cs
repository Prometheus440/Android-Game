using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	// Hearts
	[SerializeField] private Image livesImage;
	[SerializeField] private Sprite[] heartsSprite;

	//Arrows
	[SerializeField] private Image regArrowsImg;
	[SerializeField] private Image explArrowsImg;
	[SerializeField] private Image enchArrowsImg;

	// Score
	[SerializeField] private TMP_Text scoreText;
	private int currentScore = 0;

	//private Game_Manager script_gameManager;

	void Start()
	{
		//TMP_gameOverText.gameObject.SetActive(false);
		//script_gameManager = GameObject.Find("Canvas").GetComponent<Game_Manager>();

		Enemy.OnEnemyKilled += AddScore; // Subscribe
		UpdateScore(0);
	}

	void OnDestroy()
	{
		// Unsubscribe to enemy kill events
		Enemy.OnEnemyKilled -= AddScore;

	}

	public void UpdateLives(int currentLives)
	{
		currentLives = Mathf.Clamp(currentLives, 0, heartsSprite.Length - 1);
		livesImage.sprite = heartsSprite[currentLives];

		if (currentLives <= 0)
		{
			//GameOverSequence();
		}
	}
	public void UpdateScore(int score)
	{
		currentScore = score;
		scoreText.text = "Score: " + currentScore.ToString();
	}

	public void AddScore(int score)
	{
		currentScore += score;
		scoreText.text = "Score: " + currentScore.ToString();
	}

	/*
	public void GameOverSequence()
	{
		TMP_gameOverText.gameObject.SetActive(true);
		TMP_restartText.gameObject.SetActive(true);
		StartCoroutine(GameOverFlickerRoutine());
		script_gameManager.GameOver();
	}

	IEnumerator GameOverFlickerRoutine()
	{
		while (true)
		{
			TMP_gameOverText.text = "GAME OVER";
			yield return new WaitForSeconds(0.5f);
			TMP_gameOverText.text = "";
			yield return new WaitForSeconds(0.5f);
		}
	}
	*/
}