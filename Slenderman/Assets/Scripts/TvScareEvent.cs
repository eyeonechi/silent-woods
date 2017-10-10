using UnityEngine;
using System.Collections;

public class TvScareEvent : MonoBehaviour {

	public Texture2D noiseTexture;
	public Texture2D scareTexture;
	public AudioClip noiseSound;
	public AudioClip scareSound;
	public float scareTime = 3f;
	
	bool showScare = false; //private bool showscare
	
	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			/*
			renderer.material.mainTexture = scareTexture;
			audio.Stop();
			audio.loop = false;
			audio.clip = scareSound;
			audio.Play();
			*/
			showScare = true;
		}
	}
	
	void Update () 
	{
		if(showScare)
		{
			scareTime -= Time.deltaTime;
			if(scareTime <= 0)
			{
				/*
				renderer.material.mainTexture = noiseTexture;
				audio.Stop();
				audio.loop = true;
				audio.clip = noiseSound;
				audio.Play();
				*/
				showScare = false;
			}
		}
	}
}
