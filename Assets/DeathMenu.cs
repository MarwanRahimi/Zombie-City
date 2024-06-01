using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private GameObject deathMenuUI;

    private void Start()
    {
        // Make the cursor visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowDeathMenu()
    {
        // Show the death menu UI
        deathMenuUI.SetActive(true);
        // Pause the game
        Time.timeScale = 0f;
        // Make the cursor visible and unlock it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Retry()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // Resume the game
        Time.timeScale = 1f;
        // Hide the cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GoToMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
