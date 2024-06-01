using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsScript : MonoBehaviour
{
    public GameObject instructionsCanvas;
    public GameObject controllerInstructions;
    public GameObject MandKInstructions;

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
        MandKInstructions.SetActive(true);
        controllerInstructions.SetActive(false);
    }

    public void CloseInstructions()
    {
        instructionsCanvas.SetActive(false);
        MandKInstructions.SetActive(false);
        controllerInstructions.SetActive(false);
    }

    public void SwitchToControllerInstructions()
    {
        MandKInstructions.SetActive(false);
        controllerInstructions.SetActive(true);
    }

    public void SwitchToMandKInstructions()
    {
        MandKInstructions.SetActive(true);
        controllerInstructions.SetActive(false);
    }
}