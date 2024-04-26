using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuButtonFunctionality : MonoBehaviour
{
    public void StartGame()
    {
        // change scene to the game scene
        SceneManager.LoadScene("");
    }

    public void Options()
    {
        // change scene to the options scene
        SceneManager.LoadScene("");
    }

    public void Credits()
    {
        // change scene to the credits scene
        SceneManager.LoadScene("");
    }

    public void ExitGame()
    {
            Application.Quit();
    }
}
