using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPause;

    [Header("FOV Control")]
    public Slider fovSlider; 
    public Camera playerCamera;
    public TMP_Text fovValueText; 

    private const string FovPrefKey = "PlayerFOV"; 

    void Start()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPause = false;

        // Load the FOV value from PlayerPrefs or use the default camera FOV if not set
        float savedFov = PlayerPrefs.GetFloat(FovPrefKey, playerCamera.fieldOfView);

        // Initialize the slider values
        if (fovSlider != null)
        {
            fovSlider.minValue = 40;
            fovSlider.maxValue = 70;
            fovSlider.value = playerCamera.fieldOfView; // Set the slider value to the current FOV
        }

        // Update the FOV text value at the start
        if (fovValueText != null)
        {
            fovValueText.text = playerCamera.fieldOfView.ToString("F0");
        }

        // Set the player camera FOV to the saved value
        if (playerCamera != null)
        {
            playerCamera.fieldOfView = savedFov;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isPause = false; 
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Method to handle slider value changes
    public void OnFovSliderValueChanged(float value)
    {
        if (playerCamera != null)
        {
            playerCamera.fieldOfView = value;
        }

        if (fovValueText != null)
        {
            fovValueText.text = value.ToString("F0");
        }

        // Save the FOV value to PlayerPrefs
        PlayerPrefs.SetFloat(FovPrefKey, value);
        PlayerPrefs.Save();
    }
}
