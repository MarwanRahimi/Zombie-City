using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check for user input to exit the credit scene
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel"))
        {
            // Call the method to exit the credit scene and go to the main menu
            ExitCreditScene();
        }
    }

    void ExitCreditScene()
    {
        // Load the main menu scene
        SceneManager.LoadScene("Main Menu");
    }
}
