using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pauseController : MonoBehaviour {

    public Transform Player;
    public Transform pause;

    public GameObject mainCam;

    public Transform maps;
    public Transform map1;
    public Transform map2;
    public Transform map3;
    public Transform map4;
    public Transform map5;
    
    private bool lastPaperChange=false;
    bool playNarrative = true;

    public Transform winScene;

    public Transform instruction;
    
	public Transform goodDoor;
	public Transform badDoor;

    void Awake()
    {
        Cursor.visible = false;    
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			if (goodDoor && badDoor) {
				if (maps.gameObject.activeInHierarchy == false && winScene.gameObject.activeInHierarchy == false
				    && instruction.gameObject.activeInHierarchy == false && goodDoor.gameObject.activeInHierarchy == false
				    && badDoor.gameObject.activeInHierarchy == false)
					Pause ();
				else if (maps.gameObject.activeInHierarchy == true)
					mapSetting ();
				else if (winScene.gameObject.activeInHierarchy == true)
					winNarrative ();
				else if (instruction.gameObject.activeInHierarchy == true)
					loadInstruction ();
				else if (goodDoor.gameObject.activeInHierarchy == true)
					GoodDoorNot ();
				else if (badDoor.gameObject.activeInHierarchy == true)
					BadDoorNot ();
			} else {
				if (maps.gameObject.activeInHierarchy == false && winScene.gameObject.activeInHierarchy == false
					&& instruction.gameObject.activeInHierarchy == false)
					Pause ();
				else if (maps.gameObject.activeInHierarchy == true)
					mapSetting ();
				else if (winScene.gameObject.activeInHierarchy == true)
					winNarrative ();
				else if (instruction.gameObject.activeInHierarchy == true)
					loadInstruction ();
				
			}


        }

        GameObject paper = GameObject.Find("Papers");
        CollectPaper paperScript = paper.GetComponent<CollectPaper>();

       

        int paperNo = paperScript.papers;
        
        if (paperNo == 0)
        {
            map1.gameObject.GetComponent<Image>().enabled = true;
        } else if (paperNo == 1)
        {
            map1.gameObject.GetComponent<Image>().enabled = false;
            map2.gameObject.GetComponent<Image>().enabled = true;
            lastPaperChange = true;

        }
        else if (paperNo == 2)
        {
            map2.gameObject.GetComponent<Image>().enabled = false;
            map3.gameObject.GetComponent<Image>().enabled = true;
            lastPaperChange = true;

        }
        else if (paperNo == 3)
        {
            map3.gameObject.GetComponent<Image>().enabled = false;
            map4.gameObject.GetComponent<Image>().enabled = true;
            lastPaperChange = true;

        }
        else if (paperNo == 4)
        {
            map4.gameObject.GetComponent<Image>().enabled = false;
            map5.gameObject.GetComponent<Image>().enabled = true;
            lastPaperChange = true;

        }

        if (paperNo >= paperScript.papersToWin && playNarrative == true)
        {
            winNarrative();
            playNarrative = false;
        }
    }

    public void Pause()
    {
        if (pause.gameObject.activeInHierarchy == false)
        {
            Cursor.visible = true;
            pause.gameObject.SetActive(true);
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
            pause.gameObject.SetActive(false);
            Time.timeScale = 1;
            Player.GetComponent<CharacterController>().enabled = true;
            Player.GetComponent<CharLook>().enabled = true;
            Player.GetComponent<CharacterMotor>().enabled = true;
            Player.GetComponent<Footsteps>().enabled = true;


            mainCam.GetComponent<CameraLook>().enabled = true;
        }
        
    }
    public void ReturnToMain()
    {
        Time.timeScale = 1;
		Destroy(GameObject.Find("PlayerState"));
        SceneManager.LoadScene("MainMenu");
    }

    public void mapSetting ()
    {
       
        if (maps == null)
        {
            Debug.Log("no map");
        }
        else if (maps.gameObject.activeInHierarchy == false)
        {
            maps.gameObject.SetActive(true);

        }else
        {
            maps.gameObject.SetActive(false);
        }

    }

   

    public void winNarrative()
    {


        if (winScene.gameObject.activeInHierarchy == false)
        {
            Cursor.visible = true;

            winScene.gameObject.SetActive(true);
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

            winScene.gameObject.SetActive(false);
            Time.timeScale = 1;
            Player.GetComponent<CharacterController>().enabled = true;
            Player.GetComponent<CharLook>().enabled = true;
            Player.GetComponent<CharacterMotor>().enabled = true;
            Player.GetComponent<Footsteps>().enabled = true;
            mainCam.GetComponent<CameraLook>().enabled = true;
        }


    }

    public void loadInstruction()
    {
        if (instruction == null)
        {
            Debug.Log("no map");
        }
        else if (instruction.gameObject.activeInHierarchy == false)
        {
            instruction.gameObject.SetActive(true);

        }
        else
        {
            instruction.gameObject.SetActive(false);
        }

    }

	public void GoodDoorNot() {

		if (goodDoor) {
			if (goodDoor.gameObject.activeInHierarchy == false) {
				Cursor.visible = true;
				goodDoor.gameObject.SetActive (true);
				Time.timeScale = 0;
				Player.GetComponent<CharacterController> ().enabled = false;
				Player.GetComponent<CharLook> ().enabled = false;
				Player.GetComponent<CharacterMotor> ().enabled = false;
				Player.GetComponent<Footsteps> ().enabled = false;
				mainCam.GetComponent<CameraLook> ().enabled = false;
			} else {
				Cursor.visible = false;
				goodDoor.gameObject.SetActive (false);
				Time.timeScale = 1;
				Player.GetComponent<CharacterController> ().enabled = true;
				Player.GetComponent<CharLook> ().enabled = true;
				Player.GetComponent<CharacterMotor> ().enabled = true;
				Player.GetComponent<Footsteps> ().enabled = true;


				mainCam.GetComponent<CameraLook> ().enabled = true;
			}
		}
	}

	public void BadDoorNot() {
		if (badDoor) {
			if (badDoor.gameObject.activeInHierarchy == false) {
				Cursor.visible = true;
				badDoor.gameObject.SetActive (true);
				Time.timeScale = 0;
				Player.GetComponent<CharacterController> ().enabled = false;
				Player.GetComponent<CharLook> ().enabled = false;
				Player.GetComponent<CharacterMotor> ().enabled = false;
				Player.GetComponent<Footsteps> ().enabled = false;
				mainCam.GetComponent<CameraLook> ().enabled = false;
			} else {
				Cursor.visible = false;
				badDoor.gameObject.SetActive (false);
				Time.timeScale = 1;
				Player.GetComponent<CharacterController> ().enabled = true;
				Player.GetComponent<CharLook> ().enabled = true;
				Player.GetComponent<CharacterMotor> ().enabled = true;
				Player.GetComponent<Footsteps> ().enabled = true;


				mainCam.GetComponent<CameraLook> ().enabled = true;
			}
		}
	}


}
