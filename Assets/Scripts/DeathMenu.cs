using Fungus;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private GameObject deathMenuUI;
    private TextMeshProUGUI _recordText;

    private ScoreManager scoreManager;

    private void Start()
    {
        // Make the cursor visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void ShowDeathMenu()
    {
        // Show the death menu UI
        deathMenuUI.SetActive(true);
        GameObject recordObj = GameObject.FindGameObjectWithTag("Record");
        if (recordObj != null)
        {
            _recordText = recordObj.GetComponent<TextMeshProUGUI>();
            if (_recordText != null)
            {
                _recordText.text = scoreManager.currentScore.ToString();
                scoreManager.LoadScore();
                scoreManager.ResetScore();
            }
        }
        scoreManager.ResetScore();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Retry()
    {
        scoreManager.ResetScore();
        scoreManager.scoreText.text = "0";

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Destroy(player);
        }
        Time.timeScale = 1f;
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
        Application.Quit();
    }
}