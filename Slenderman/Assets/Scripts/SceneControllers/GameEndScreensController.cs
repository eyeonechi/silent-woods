using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndScreensController : MonoBehaviour {

    public Text resultText;

    void Start()
    {
        // Display text based on last game result
        if (InGameController.lastGameWon)
        {
            this.resultText.text = "You Won!";
        }
        else
        {
            this.resultText.text = "You Lost!";
        }
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

	public void OpenMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void Retry()
	{
		SceneManager.LoadScene("Game");
	}

}
