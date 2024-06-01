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
    public Slider fovSlider; // Reference to the slider
    public Camera playerCamera; // Reference to the player camera
    public TMP_Text fovValueText; // Reference to the TextMesh Pro text

    private const string FovPrefKey = "PlayerFOV"; // Key to store the FOV value

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPause = false; // Ensure isPause is false when the game starts

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

    // Update is called once per frame
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
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Show the cursor
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isPause = false; // Ensure isPause is reset when going to the main menu
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
