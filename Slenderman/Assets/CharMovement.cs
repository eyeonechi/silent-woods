using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : MonoBehaviour {
    CharacterController player;
    public float speed = 2f;
    private float sensitivity = 15f;

    private float moveFB;
    private float moveLR;
    private float rotX;
    private float rotY;

    public GameObject Eyes;


	// Use this for initialization
	void Start () {
        player = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        moveFB = Input.GetAxis("Vertical") * speed;
        moveLR = Input.GetAxis("Horizontal") * speed;

        rotX = Input.GetAxis("Mouse X") * sensitivity;

        Vector3 movement = new Vector3(moveLR, 0, moveFB);
        movement = transform.rotation * movement;

        transform.Rotate(0, rotX, 0);
        player.Move(movement * Time.deltaTime);

	}
}
