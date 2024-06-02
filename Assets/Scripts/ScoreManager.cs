using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance { get { return instance; } }

    private const string PLAYER_PREFS_KEY = "PlayerScore";

    public int currentScore = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI recordText;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        GameObject scoreTextObject = GameObject.Find("Canvas/ScoreText");

        if (scoreTextObject != null)
        {
            scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("ScoreText GameObject not found! Make sure it exists in the hierarchy.");
        }

        LoadScore();
    }

    // Load score from PlayerPrefs
    public void LoadScore()
    {
        if (!PlayerPrefs.HasKey(PLAYER_PREFS_KEY))
        {
            PlayerPrefs.SetInt(PLAYER_PREFS_KEY, 0);
            currentScore = 0;
        }
        else
        {
            currentScore = PlayerPrefs.GetInt(PLAYER_PREFS_KEY);

            // Update the scoreText if it's not null
            if (scoreText != null)
            {
                scoreText.text = "Score: " + currentScore;
            }
            if (recordText != null)
            {
                recordText.text = currentScore.ToString();
                ResetScore();
            }
        }
    }

    // Add score and save it
    public void AddScore(int amount)
    {
        currentScore += amount;

        Debug.Log("Score is: " + currentScore);

        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
        if (recordText != null)
        {
            recordText.text = currentScore.ToString();
        }
        PlayerPrefs.SetInt(PLAYER_PREFS_KEY, currentScore);
        PlayerPrefs.Save();
    }

    // Reset score to zero
    public void ResetScore()
    {
        currentScore = 0;
        PlayerPrefs.SetInt(PLAYER_PREFS_KEY, currentScore);
        PlayerPrefs.Save();

        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
}