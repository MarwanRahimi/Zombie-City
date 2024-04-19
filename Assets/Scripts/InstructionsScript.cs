using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsScript : MonoBehaviour
{
    public GameObject instructionsCanvas;
    public GameObject controllerInstructions;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseInstructions();
        }
    }

    public void OpenInstructions()
    {
        instructionsCanvas.SetActive(true);
    }

    public void CloseInstructions()
    {
        instructionsCanvas.SetActive(false);
    }

    public void SwitchToControllerInstructions()
    {
        instructionsCanvas.SetActive(false);
        controllerInstructions.SetActive(true);
    }

    public void SwitchToMandKInstructions()
    {
        instructionsCanvas.SetActive(true);
        controllerInstructions.SetActive(false);
    }

}