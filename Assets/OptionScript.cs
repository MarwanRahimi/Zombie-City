using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class OptionScript : MonoBehaviour
{
    public TMP_Dropdown ResDropdown;
    public Toggle FullScreenToggle;
    public Toggle VsyncToggle;

    Resolution[] AllResolutions;
    bool IsVsync;
    bool IsFullScreen;
    int SelectedResolution;
    List<Resolution> SelectedResolutionList = new List<Resolution>();

    // Start is called before the first frame update
    void Start()
    {
        IsFullScreen = true;
        AllResolutions = Screen.resolutions;

        List<string> resolutionStringList = new List<string>();
        string newRes;
        foreach(Resolution res in AllResolutions)
        {
            newRes = res.width.ToString() + " x " + res.height.ToString();
            if(!resolutionStringList.Contains(newRes))
            {
                resolutionStringList.Add(newRes);
                SelectedResolutionList.Add(res);
            }
            
        }

        ResDropdown.AddOptions(resolutionStringList);
    }

    public void ChangeResolution()
    {
        SelectedResolution = ResDropdown.value;
        Screen.SetResolution(SelectedResolutionList[SelectedResolution].width, SelectedResolutionList[SelectedResolution].height, IsFullScreen);
    }

    public void ChangeFullScreen()
    {
        IsFullScreen = FullScreenToggle.isOn;
        Screen.SetResolution(SelectedResolutionList[SelectedResolution].width, SelectedResolutionList[SelectedResolution].height, IsFullScreen);
    }

    public void ChangeVsync()
    {
        if (VsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
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

    public void ExitOptionScene()
    {
        // Load the main menu scene
        SceneManager.LoadScene("Main Menu");
    }
}