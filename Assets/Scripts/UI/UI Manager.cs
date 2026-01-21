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

	void Start()
	{
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
}