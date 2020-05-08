using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {

    float batteryLife = 15.0f;
    private float power = 0.0f;
    private bool on = false;

	// Use this for initialization
	void Start () {
        power = batteryLife;
	}
	
	// Update is called once per frame
	void Update () {
		if (on == true)
        {
            power -= Time.deltaTime;
           // GetComponent.<Light>().enabled = true;
        }
        else
        {

        }
	}
}
