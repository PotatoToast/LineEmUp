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

    public void SetResolution(int option)
    {
        Debug.Log(option);
        Resolution[] resolutions = Screen.resolutions;
        int maxWidth = resolutions[resolutions.Length - 1].width;
        switch (option)
        {
            case 0:
                if (maxWidth < 2560) break;
                Screen.fullScreenMode = FullScreenMode.Windowed;
                SetResolutionScale(2560);
                break;
            case 1:
                if (maxWidth < 1920) break;
                Screen.fullScreenMode = FullScreenMode.Windowed;
                SetResolutionScale(1920);
                break;
            case 2:
                if (maxWidth < 1600) break;
                Screen.fullScreenMode = FullScreenMode.Windowed;
                SetResolutionScale(1600);
                break;
            case 3:
                if (maxWidth < 1280) break;
                Screen.fullScreenMode = FullScreenMode.Windowed;
                SetResolutionScale(1280);
                break;
            case 4:
                SetResolutionScale(resolutions[resolutions.Length - 1].width);
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

                break;
            default:
                Debug.Log("Option " + option + " not found");
                break;
        }
    }

    public void SetResolutionScale(int resolution)
    {
        Screen.SetResolution(resolution, resolution * 9 / 16, false);
        //Display.main.SetRenderingResolution(resolution, resolution * 9 / 16);
    }
}
