using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour {
    private float sensitivity = 15f;
    private float rotY;

   
    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;
   
	
	// Update is called once per frame
	void Update () {

        
        rotY += Input.GetAxis("Mouse Y") * sensitivity;
        rotY = Mathf.Clamp(rotY, minimumY, maximumY);
        transform.localEulerAngles = new Vector3(-rotY, transform.localEulerAngles.y, 0);


    }
}
