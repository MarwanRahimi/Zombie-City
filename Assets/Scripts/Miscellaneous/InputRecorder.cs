using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRecorder : MonoBehaviour {

    // TO DO:
    // Write the double list out to a file if M is pressed, so that it can later be loaded in by a InputSimulator

    List<List<string>> inputRecordedData = new List<List<string>>();

    // Update is called once per frame
    void Update() {

        RecordInputs();

        if (Input.GetKeyDown(KeyCode.M)) {
            for (int i = 0; i < inputRecordedData.Count; i++) {
                Debug.Log("---------------------------------------------------------");
                Debug.Log("Mouse X: " + inputRecordedData[i][0]);
                Debug.Log("Mouse Y: " + inputRecordedData[i][1]);
                Debug.Log("Mouse button pressed: " + inputRecordedData[i][2]);
                Debug.Log("W pressed: " + inputRecordedData[i][3]);
                Debug.Log("Shift pressed: " + inputRecordedData[i][4]);
                Debug.Log("2 pressed: " + inputRecordedData[i][5]);
                Debug.Log("P pressed: " + inputRecordedData[i][6]);
                Debug.Log("L pressed: " + inputRecordedData[i][7]);
            }
        }
    }

    private void RecordInputs() {
        List<string> inputsThisFrame = new List<string>();

        inputsThisFrame.Add(Input.GetAxis("Mouse X").ToString());
        inputsThisFrame.Add(Input.GetAxis("Mouse Y").ToString());
        inputsThisFrame.Add(Input.GetMouseButton(0).ToString());
        inputsThisFrame.Add(Input.GetKey(KeyCode.W).ToString());
        inputsThisFrame.Add(Input.GetKey(KeyCode.LeftShift).ToString());
        inputsThisFrame.Add(Input.GetKeyDown(KeyCode.Alpha2).ToString());
        inputsThisFrame.Add(Input.GetKeyDown(KeyCode.P).ToString());
        inputsThisFrame.Add(Input.GetKeyDown(KeyCode.L).ToString());

        inputRecordedData.Add(inputsThisFrame);


    }
}
