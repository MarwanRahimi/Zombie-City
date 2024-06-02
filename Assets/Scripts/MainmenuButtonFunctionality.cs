using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuButtonFunctionality : MonoBehaviour
{
    public void StartGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Destroy(player);
        }
        // change scene to the game scene
        SceneManager.LoadScene("OpeningCutScene");
    }

    public void Options()
    {
        // change scene to the options scene
        SceneManager.LoadScene("Option");
    }

    public void Credits()
    {
        // change scene to the credits scene
        SceneManager.LoadScene("Credit");
    }

    public void ExitGame()
    {
            Application.Quit();
    }
}
