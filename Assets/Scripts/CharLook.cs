using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharLook : MonoBehaviour {
    public float sensitivityX = 15f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);

    }
}
