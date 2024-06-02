using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance { get { return instance; } }

    private const string PLAYER_PREFS_KEY = "PlayerScore";

    private int currentScore = 0;

    private TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        scoreText = GameObject.Find("Canvas/ScoreText").GetComponent<TextMeshProUGUI>();
        
        if(!PlayerPrefs.HasKey(PLAYER_PREFS_KEY))
        {
            PlayerPrefs.SetInt(PLAYER_PREFS_KEY, 0);
            currentScore = 0;
        }
    }


    public void AddScore(int amount)
    {
        if (!PlayerPrefs.HasKey(PLAYER_PREFS_KEY))
        {
            currentScore = 0;
        }
        else
        {
            currentScore = PlayerPrefs.GetInt(PLAYER_PREFS_KEY);
        }

        currentScore += amount;

        Debug.Log("Score is: " + currentScore);

        if (scoreText)
        {
            scoreText.text = "Score: " + currentScore;
        }

        PlayerPrefs.SetInt(PLAYER_PREFS_KEY, currentScore);
        PlayerPrefs.Save();
    }

}
