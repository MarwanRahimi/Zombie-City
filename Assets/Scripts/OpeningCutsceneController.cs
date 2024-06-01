using UnityEngine;
using Fungus;

public class OpeningCutsceneController : MonoBehaviour
{
    // Reference to the Fungus Flowchart containing the opening cutscene
    public Flowchart openingCutsceneFlowchart;

    // Boolean variable to track whether the cutscene has been played
    private bool hasPlayedCutscene = false;

    void Start()
    {
        // Ensure the Flowchart reference is set
        if (openingCutsceneFlowchart == null)
        {
            Debug.LogError("OpeningCutsceneController: Flowchart reference is not set.");
            return;
        }

        // Check if the cutscene has been played
        if (!hasPlayedCutscene)
        {
            // Play the cutscene
            PlayOpeningCutscene();
        }
    }

    // Method to play the opening cutscene
    private void PlayOpeningCutscene()
    {
        // Execute the opening cutscene block in the Flowchart
        openingCutsceneFlowchart.ExecuteBlock("StartOpeningCutscene");

        // Update the boolean variable to indicate that the cutscene has been played
        hasPlayedCutscene = true;
    }
}
