using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
    }

    //Opens pause panel and stops game
    public void Pause()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    //Closes pause panel and starts game
    public void Continue()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
