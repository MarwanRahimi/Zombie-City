using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    
    // Configuration parameters
    [SerializeField] int targetFramerate = 60;

    void Start() {
        Application.targetFrameRate = targetFramerate;
    }

    public void LoadFirstLevel() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level 1");
    }

    public void LoadCreditsScreen() {
        SceneManager.LoadScene("Credits Screen");
    }

    public void LoadStartScreen() {
        //Time.timeScale = 1;
        SceneManager.LoadScene("Start Screen");
    }

    public void LoadTutorialScreen() {
        SceneManager.LoadScene("Tutorial Screen");
    }

    public void ReloadLevel() {
        Time.timeScale = 1;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        
    }

    public void QuitGame() {
        Time.timeScale = 1;
        Application.Quit();
    }

}
