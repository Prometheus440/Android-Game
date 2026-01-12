using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	// Hearts
	[SerializeField] private Image img_lives;
	[SerializeField] private Sprite[] sp_lives;

	//Arrows
	[SerializeField] private Image img_regArrows;
	[SerializeField] private Sprite[] sp_regArrows;
	[SerializeField] private Image img_explArrows;
	[SerializeField] private Sprite[] sp_explArrows;
	[SerializeField] private Image img_enchArrows;
	[SerializeField] private Sprite[] sp_enchArrows;

	//private Game_Manager script_gameManager;

	void Start()
	{
		//TMP_gameOverText.gameObject.SetActive(false);
		//script_gameManager = GameObject.Find("Canvas").GetComponent<Game_Manager>();
	}

	public void UpdateLives(int currentLives)
	{
		img_lives.sprite = sp_lives[currentLives];

		if (currentLives == 0)
		{
			//GameOverSequence();
		}
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