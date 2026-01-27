using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
// using UnityEditor.U2D.Animation;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {
        
    }

    //Opens pause panel and stops game
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    //Closes pause panel and starts game
    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
    }

    public void CloseSettings()
    {
        StartCoroutine(DelayPause());
    }

    IEnumerator DelayPause()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        pauseMenu.SetActive(true);
    }

    public void Exit()
    {
        
    }
}
