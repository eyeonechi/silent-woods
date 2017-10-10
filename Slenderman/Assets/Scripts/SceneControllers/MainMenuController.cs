using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    
    public void StartGame()
    {
		SceneManager.LoadScene ("Game");
    }

    public void OpenInstructions()
    {
		SceneManager.LoadScene ("Instructions");
    }

    public void OpenOptions()
    {
		SceneManager.LoadScene ("Options");
    }

	public void ExitGame()
	{
		Application.Quit ();
	}
}
