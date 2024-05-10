using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovementFPS : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

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



    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody>();
       rb.freezeRotation = true;


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

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); // get X inputs 
        verticalInput = Input.GetAxisRaw("Vertical"); // get Y inputs

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();
            Debug.Log("Jumping");

            Invoke(nameof(ResetJump), jumpCoolDown);
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
}
