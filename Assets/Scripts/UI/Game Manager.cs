using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private bool gameOver;

	void Update()
	{
		// Restart game
	}

	public void GameOver()
	{
		gameOver = true;
	}
}