using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	public bool pauseAtStart = true;
	public GameObject player;
	public GameObject menuCamera;
	public GameObject menuPanel;
	public Text button;

	void Start()
	{
		if(pauseAtStart)
		{
			player.SetActive(false);
		}
		else
		{
			menuCamera.SetActive(false);
			menuPanel.SetActive(false);
		}
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			menuCamera.SetActive(true);
			menuPanel.SetActive(true);
			button.text = "Resume";
			player.SetActive(false);
		}
	}

	public void StartGame()
	{
		menuCamera.SetActive(false);
		menuPanel.SetActive(false);
		player.SetActive(true);
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}
}
