using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player GameObject
    public float rotationSpeed = 5f; // Speed of rotation

    private Vector3 offset; // Offset between player and camera

    void Start()
    {
        // Calculate the initial offset between player and camera
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Calculate the desired rotation based on player's position
        Quaternion rotation = Quaternion.Euler(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

        // Rotate the offset around player's position
        offset = rotation * offset;

        // Set the camera's position to the new rotated position
        //transform.position = player.position + offset;

        // Make the camera look at the player
        transform.LookAt(player.position);
    }
}