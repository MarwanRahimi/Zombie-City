using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovementFPS : MonoBehaviour
{

    [Header("Movement")]
    private float moveSpeed = 0f;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.C; 

    [Header("Ground Check")]//check if the player is on the ground
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 movementDirection;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private float groundCheckRadius = 0.5f;

    Rigidbody rb;

    private MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }


    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody>();
       rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;

    }

    // Update is called once per frame
    void Update()
    {

        if (groundCheck != null)
        {
            grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, whatIsGround);
        }

        if (grounded )//check the player grounded state and handle drag
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        MyInput();
        speedControl();
        StateHandler();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); // get X inputs 
        verticalInput = Input.GetAxisRaw("Vertical"); // get Y inputs

        if(Input.GetKey(jumpKey) && readyToJump && grounded && state != MovementState.crouching)
        {
            readyToJump = false;

            Jump();
            //Debug.Log("Jumping");

            Invoke(nameof(ResetJump), jumpCoolDown);
        }

        //Start crouching
        if(Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        //Stop crouching
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void MovePlayer()
    {

        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;//calculate movement

        if(grounded)
            rb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if(!grounded)
            rb.AddForce(movementDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

    }

    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); //flat velocity of rigidbody

        //limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

    }

    private void OnDrawGizmos()
    {
        if(groundCheck != null)
        {
            Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void Jump()
    {
        //reset Y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        Debug.Log("Jumping");
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void StateHandler()
    {
        // Debug.Log("Current Speed: " + moveSpeed);
        // Crouching
        if (grounded && Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        // Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        // Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        // Air
        else
        {
            state = MovementState.air;
        }
    }

}
