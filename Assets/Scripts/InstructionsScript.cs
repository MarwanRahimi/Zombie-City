using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsScript : MonoBehaviour
{
    public GameObject instructionsCanvas;

    public void OpenInstructions()
    {
        instructionsCanvas.SetActive(true);
    }
}
