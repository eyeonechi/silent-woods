using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEffect : MonoBehaviour {

	private MovieTexture movie;

	// Use this for initialization
	void Start () {
		Renderer r = GetComponent<Renderer> ();
		movie = (MovieTexture) r.material.mainTexture;
		movie.loop = true;
	}

	// Update is called once per frame
	void Update () {
		movie.Play ();
	}
}
