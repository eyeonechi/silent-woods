using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : MonoBehaviour {

	public Animator anim;
	public Rigidbody rigidbody;

	private float inputH;
	private float inputV;

	// Use this for initialization
	void Start () { 
		anim = GetComponent<Animator> ();
		rigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1")) {
			anim.Play ("crawl");
		}

		inputH = Input.GetAxis ("Horizontal");
		inputV = Input.GetAxis ("Vertical");

		anim.SetFloat ("inputH", inputH);
		anim.SetFloat ("inputV", inputV);

		float moveX = inputH * 20f * Time.deltaTime;
		float moveY = inputV * 50f * Time.deltaTime;
	}
}
