using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ResolutionManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    public Toggle FullScreenToggle;
    bool isFullScreen;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolution;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        isFullScreen = true;
        resolutions = Screen.resolutions;
        filteredResolution = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        Debug.Log("RefreshRate: " + currentRefreshRate);

        for (int i = 0; i < resolutions.Length; i++)
        {
            Debug.Log("Resolution: " + resolutions[i]);
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filteredResolution.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolution.Count;i++)
        {
            string resolutionOption = filteredResolution[i].width + "x" + filteredResolution[i].height + " " + filteredResolution[i].refreshRate + " Hz";
            options.Add(resolutionOption);
            if (filteredResolution[i].width == Screen.width && filteredResolution[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolution[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    public void ChangeFullScreen()
    {
        isFullScreen = FullScreenToggle.isOn;
    }
}
