using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMotion : MonoBehaviour {
    // Constants
    private const int NORMAL_SPEED = 10;
    private const int SPRINT_SPEED = 50;

    // Inspector Variables
    public Rigidbody rb;
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    public bool flag = true;

    // Private variables
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float speed;

    private Vector3 moveDirection = Vector3.zero;
    private int antiBunnyHopFactor = 1;
    private float jumpSpeed = 100.0f;
    private float gravity = 5.0f;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {
        
        speed = NORMAL_SPEED;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            flag = !flag;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = SPRINT_SPEED;
        }

        ToggleCursor(speed);
    }



    // Captures and releases the mouse in the game scene when the Control button is pressed
    void ToggleCursor(float speed)
    {
        int jumpTimer = antiBunnyHopFactor;
        if (flag)
        {
            Cursor.lockState = CursorLockMode.Locked;
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

            // If both horizontal and vertical are used simultaneously, limit speed (if allowed), so the total doesn't exceed normal move speed
            float inputModifyFactor = (inputX != 0.0f && inputZ != 0.0f) ? .7071f : 1.0f;


            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            this.transform.eulerAngles = new Vector3(pitch, yaw, transform.eulerAngles.z);
            moveDirection = new Vector3(inputX * inputModifyFactor, 0, inputZ * inputModifyFactor);

          //  moveDirection.y -= gravity * Time.deltaTime;


            // Jump! But only if the jump button has been released and player has been grounded for a given number of frames
            if (Input.GetKeyUp(KeyCode.Space))
                moveDirection.y = jumpSpeed;
            else if (jumpTimer >= antiBunnyHopFactor)
            {
                
                jumpTimer = 0;
            }
            transform.Translate(moveDirection);
            Debug.Log("jumpTimer = " + jumpTimer);
            Debug.Log("antibunny = " + antiBunnyHopFactor);

            // Apply gravity
           
         


        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

