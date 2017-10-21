using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class narrativeController : MonoBehaviour {

    public void intoGame()
    {
        SceneManager.LoadScene("Game");
    }
}
