using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerobj;
    private Rigidbody rb;
    private PlayerMovementFPS pm;
    private CharacterStats characterStats;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTime;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.F;
    private float horizontalInput;
    private float verticalInput;

    private bool sliding;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementFPS>();
        characterStats = GetComponent<CharacterStats>(); // Ensure this component is attached to the same GameObject

        startYScale = playerobj.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
            StartSlide();

        if (Input.GetKeyUp(slideKey) && sliding)
            StopSlide();
    }

    private void FixedUpdate()
    {
        if (sliding)
        {
            SlidingMovement();
        }
    }

    private void StartSlide()
    {
        sliding = true;

        playerobj.localScale = new Vector3(playerobj.localScale.x, slideYScale, playerobj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTime = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

        slideTime -= Time.deltaTime;

        // Drain stamina while sliding
        characterStats.PlayerSliding(characterStats.SlideStaminaDrain);

        if (slideTime <= 0 || characterStats.stamina.CurrentVal <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        sliding = false;

        playerobj.localScale = new Vector3(playerobj.localScale.x, startYScale, playerobj.localScale.z);
    }
}
