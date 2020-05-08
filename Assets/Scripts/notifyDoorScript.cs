using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notifyDoorScript : MonoBehaviour {

	public Transform goodDoor;
	public Transform badDoor;
	public GameObject Player;
	public GameObject mainCam;

	// Update is called once per frame
	public void GoodDoorNot() {
		if (goodDoor.gameObject.activeInHierarchy == false)
		{
			Cursor.visible = true;
			goodDoor.gameObject.SetActive(true);
			Time.timeScale = 0;
			Player.GetComponent<CharacterController>().enabled = false;
			Player.GetComponent<CharLook>().enabled = false;
			Player.GetComponent<CharacterMotor>().enabled = false;
			Player.GetComponent<Footsteps>().enabled = false;
			mainCam.GetComponent<CameraLook>().enabled = false;
		}
		else
		{
			Cursor.visible = false;
			goodDoor.gameObject.SetActive(false);
			Time.timeScale = 1;
			Player.GetComponent<CharacterController>().enabled = true;
			Player.GetComponent<CharLook>().enabled = true;
			Player.GetComponent<CharacterMotor>().enabled = true;
			Player.GetComponent<Footsteps>().enabled = true;


			mainCam.GetComponent<CameraLook>().enabled = true;
		}
	}

	public void BadDoorNot() {
		if (badDoor.gameObject.activeInHierarchy == false)
		{
			Cursor.visible = true;
			badDoor.gameObject.SetActive(true);
			Time.timeScale = 0;
			Player.GetComponent<CharacterController>().enabled = false;
			Player.GetComponent<CharLook>().enabled = false;
			Player.GetComponent<CharacterMotor>().enabled = false;
			Player.GetComponent<Footsteps>().enabled = false;
			mainCam.GetComponent<CameraLook>().enabled = false;
		}
		else
		{
			Cursor.visible = false;
			badDoor.gameObject.SetActive(false);
			Time.timeScale = 1;
			Player.GetComponent<CharacterController>().enabled = true;
			Player.GetComponent<CharLook>().enabled = true;
			Player.GetComponent<CharacterMotor>().enabled = true;
			Player.GetComponent<Footsteps>().enabled = true;


			mainCam.GetComponent<CameraLook>().enabled = true;
		}
	}
}
