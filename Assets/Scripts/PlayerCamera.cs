using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX; // X sensitivity 
    public float sensY; // Y sensitivity 

    public Transform orientation; // Player orientation

    float xRotation; // Camera X rotation
    float yRotation; // Camera Y rotation

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Set the cursor at the middle of the screen
        Cursor.visible = false; // Make the cursor invisible
    }

    // Update is called once per frame
    void Update()
    {
        // If orientation is null, try to find the player object with the orientation
        if (orientation == null)
        {
            FindPlayerOrientation();
        }

        // If orientation is still null, do not proceed with camera update logic
        if (orientation != null)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX; // Get mouse input
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY; // Get mouse input

            yRotation += mouseX; // Set camera rotation on player Y direction
            xRotation -= mouseY; // Set camera rotation on player X direction

            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit camera X rotation 

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    // Method to find the player's orientation transform
    void FindPlayerOrientation()
    {
        GameObject player = GameObject.FindWithTag("Player"); // Assumes the player has the tag "Player"
        if (player != null)
        {
            orientation = player.transform; // Set the player's transform as orientation
        }
    }

    // Method to set the player's orientation transform from an external script
    public void SetPlayerOrientation(Transform newOrientation)
    {
        orientation = newOrientation;
    }
}
