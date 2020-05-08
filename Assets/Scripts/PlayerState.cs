using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

	public static PlayerState instance;

	public float x;
	public float y;
	public float z;
	public float hp;
	public int papers;
	public bool[] papersCollected;
	public bool[] keysCollected;

	public float enemyTeleportDistance;
	public float enemyTeleportRate;

	void Awake() {
		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = this;
			papersCollected = new bool[5];
			keysCollected = new bool[4];

			enemyTeleportRate = 10f;
			enemyTeleportDistance = 50.0f;

		} else if (instance != this) {
			Destroy (gameObject);
		}
	}

	public void ResetState() {
		x = 156f;
		y = 7f;
		z = 187f;
		hp = 100f;
		papers = 0;
		papersCollected = new bool[5];
		keysCollected = new bool[4];
		enemyTeleportRate = 10f;
		enemyTeleportDistance = 50.0f;
	}

}
