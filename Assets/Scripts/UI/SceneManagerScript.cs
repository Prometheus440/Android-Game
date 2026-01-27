using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void ChangeScene(string sceneName) //Changes scene to parameter name
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }
    public void CloseGame() //CLoses game
    {
        Application.Quit();
    }
}
