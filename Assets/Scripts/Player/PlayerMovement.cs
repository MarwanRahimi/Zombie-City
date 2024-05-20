using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController playerController;

    private Animator playerAnimator;

    public float playerMovementSpeed = 12.0f;

    void Start()
    {
        playerController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        float xInput = Input.GetAxis("Horizontal"); // D -> 1 / A -> -1
        float zInput = Input.GetAxis("Vertical");

        Vector3 forwardMovementDirection = transform.forward * zInput * playerMovementSpeed * Time.deltaTime;
        Vector3 rightMovementDirection = transform.right * xInput * playerMovementSpeed * Time.deltaTime; // Time.deltaTime makes it framerate independant

        if (xInput == 0 && zInput == 0)
        {
            //playerAnimator.SetBool("IsMoving", false);
        }
        if (xInput != 0 || zInput != 0)
        {
            //playerAnimator.SetBool("IsMoving", true);

        }


        playerController.Move(forwardMovementDirection);
        playerController.Move(rightMovementDirection);



    }
}
