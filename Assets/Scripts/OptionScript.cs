using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionScript : MonoBehaviour
{
    public Toggle fullscreenToggle, vsyncToggle;

    public List<ResolutionItem> resolutions = new List<ResolutionItem>();

    // Start is called before the first frame update
    void Start()
    {
        fullscreenToggle.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncToggle.isOn = false;
        }
        else
        {
            vsyncToggle.isOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for user input to exit the option scene
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel"))
        {
            // Call the method to exit the option scene and go to the main menu
            ExitOptionScene();
        }
    }

    public void ApplyGraphics()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        
        if (vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    public void ExitOptionScene()
    {
        // Load the main menu scene
        SceneManager.LoadScene("Main Menu");
    }
}

[System.Serializable]
public class ResolutionItem
{
    public int horizontal, vertical;


}