using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour {

	CharacterController cc;
	
	void Start ()	
 	{
		cc = GetComponent<CharacterController>();
	}
	
	void Update ()	
 	{
        AudioSource audio = GetComponent<AudioSource>();
        if (cc.isGrounded == true && cc.velocity.magnitude > 2f && audio.isPlaying == false)
		{
            audio.volume = Random.Range(0.8f, 1);
			audio.pitch = Random.Range(0.8f, 1.1f);
			audio.Play();
		}
	}
}
