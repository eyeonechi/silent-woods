using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour {

	public bool[] keys;
	
	void Start()
	{
		keys = PlayerState.instance.keysCollected;
		keys [0] = true;

		if (SceneManager.GetActiveScene ().name.Equals ("Inside")) {
			if (keys [0] == true) {
				((DoorScript) GameObject.Find("Door_Hallway").GetComponent("DoorScript")).ChangeDoorState();
			}
			if (keys [1] == true) {
				GameObject.Destroy (GameObject.Find ("Key1"));
				((DoorScript) GameObject.Find("Door_Bathroom").GetComponent("DoorScript")).ChangeDoorState();
			}
			if (keys [2] == true) {
				GameObject.Destroy (GameObject.Find ("Key2"));
				((DoorScript) GameObject.Find("Door_Bedroom").GetComponent("DoorScript")).ChangeDoorState();
			}
			if (keys [3] == true) {
				GameObject.Destroy (GameObject.Find ("Key3"));
				((DoorScript) GameObject.Find("Door_Basement").GetComponent("DoorScript")).ChangeDoorState();
			}
		}
	}

	public void SaveState() {
		if (PlayerState.instance != null) {
			PlayerState.instance.keysCollected = keys;
		}
	}
	
}
