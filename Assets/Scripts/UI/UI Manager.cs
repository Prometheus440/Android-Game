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
	[SerializeField] private TMP_Text highscoreText;
	private int highscore = 0;

	// Game Over
	[SerializeField] private TMP_Text gameOverText;
	[SerializeField] private GameManager gameManager;
	[SerializeField] private GameObject gameOverButton;

	void Start()
	{
		gameOverText.enabled = false;
		gameOverButton.SetActive(false);
		Enemy.OnEnemyKilled += AddScore; // Subscribe
		UpdateScore(0);

		// Load highscore from PlayerPrefs
		highscore = PlayerPrefs.GetInt("Highscore", 0);
		UpdateHighscore();
		UpdateScore(0);
	}

	private void Update()
	{
		if (gameManager.gameOver == true)
		{
			gameOverButton.SetActive(true);
			gameOverText.enabled = true;
			SaveHighscore();
		}
	}

	void OnDestroy()
	{
		// Unsubscribe to enemy kill events
		Enemy.OnEnemyKilled -= AddScore;
	}

	public void UpdateLives(int currentLives)
	{
		// Update UI
		currentLives = Mathf.Clamp(currentLives, 0, heartsSprite.Length - 1);
		livesImage.sprite = heartsSprite[currentLives];
	}
	public void UpdateScore(int score)
	{
		currentScore = score;
		scoreText.text = "Score: " + currentScore.ToString();

		// Check for highscore
		if (currentScore > highscore)
		{
			highscore = currentScore;
			UpdateHighscore();
		}
	}

	public void AddScore(int score)
	{
		currentScore += score;
		scoreText.text = "Score: " + currentScore.ToString();

		// Check for highscore
		if (currentScore > highscore)
		{
			highscore = currentScore;
			UpdateHighscore();
		}
	}

	private void UpdateHighscore()
	{
		if (highscoreText != null)
		{
			highscoreText.text = "Highscore: " + highscore.ToString();
		}
	}

	void SaveHighscore()
	{
		if (currentScore > PlayerPrefs.GetInt("Highscore", 0))
		{
			PlayerPrefs.SetInt("Highscore", currentScore);
			PlayerPrefs.Save(); // Force save to disk
		}
	}
}