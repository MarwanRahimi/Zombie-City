using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX; //X sensivity 
    public float sensY; //Y sensivity 

    public Transform orientation; // player orientation

    float xRotation; // Camera X rotation
    float yRotation; // Camera Y rotation

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // set the cursor at the middle of the screen
        Cursor.visible = false; // make the cursor invisible
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX; // get mouse input
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY; // get mouse input

        yRotation += mouseX; // set camera rotation on player Y direction
        xRotation -= mouseY; // set camera rotation on player X direction

        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // limit camera X rotation 

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }
}
