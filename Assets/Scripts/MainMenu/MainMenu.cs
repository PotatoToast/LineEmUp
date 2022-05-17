using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        //Play Game; 
    }
    public void QuitGame()
    {
        Debug.Log("Quit Successful");
        Application.Quit();
    }
}
