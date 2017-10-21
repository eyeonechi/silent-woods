using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
