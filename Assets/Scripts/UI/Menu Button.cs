using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonScript : MonoBehaviour
{
    public void EnableMenu() //Calls the trigger bringing the menu to the screen
    {
        GetComponent<Animator>().ResetTrigger("Disable");
        GetComponent<Animator>().SetTrigger("Enable");
    }

    public void DisableMenu() //Calls the trigger removing the menu from the screen
    {
        GetComponent<Animator>().ResetTrigger("Enable");
        GetComponent<Animator>().SetTrigger("Disable");
    }
}
