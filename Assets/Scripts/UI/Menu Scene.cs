using TMPro;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
	[SerializeField] private TMP_Text highscoreText;

	void Start()
	{
		int highscore = PlayerPrefs.GetInt("Highscore", 0);
		highscoreText.text = "Highscore: " + highscore.ToString();
	}
}
