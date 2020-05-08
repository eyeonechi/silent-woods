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
    

    void Awake()
    {
        Cursor.visible = false;    
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (maps.gameObject.activeInHierarchy == false && winScene.gameObject.activeInHierarchy == false) Pause();
            else if (maps.gameObject.activeInHierarchy == true) mapSetting();
            else if (winScene.gameObject.activeInHierarchy == true) winNarrative();

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

        if (paperNo == paperScript.papersToWin && playNarrative == true)
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

            mainCam.GetComponent<CameraLook>().enabled = false;
        }
        else
        {
            Cursor.visible = false;
            pause.gameObject.SetActive(false);
            Time.timeScale = 1;
            Player.GetComponent<CharacterController>().enabled = true;
            Player.GetComponent<CharLook>().enabled = true;

            mainCam.GetComponent<CameraLook>().enabled = true;
        }
        
    }
    public void ReturnToMain()
    {
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

    void OnGUI()
    {
        if (lastPaperChange == true)
        {
          //  GUI.Box(new Rect((Screen.width * 0.5f) - 140, (Screen.height * 0.5f) - 60, 300, 25), "Map has been updated!");
            //  lastPaperChange = false;
        } 
    }

    public void winNarrative()
    {


        if (winScene.gameObject.activeInHierarchy == false)
        {
            winScene.gameObject.SetActive(true);
            Time.timeScale = 0;
            Player.GetComponent<CharacterController>().enabled = false;
            Player.GetComponent<CharLook>().enabled = false;

            mainCam.GetComponent<CameraLook>().enabled = false;
        }
        else
        {
            winScene.gameObject.SetActive(false);
            Time.timeScale = 1;
            Player.GetComponent<CharacterController>().enabled = true;
            Player.GetComponent<CharLook>().enabled = true;

            mainCam.GetComponent<CameraLook>().enabled = true;
        }


    }


}
