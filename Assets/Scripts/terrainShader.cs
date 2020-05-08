using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainShader : MonoBehaviour {
    public Shader shader;
	// Use this for initialization
	void Awake () {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.shader = shader;
    }
	
	
}
