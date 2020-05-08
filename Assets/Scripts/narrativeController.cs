using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class narrativeController : MonoBehaviour {

    public Transform maps;
    public Transform narrative;
    public void intoGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void mapSetting()
    {

        if (maps == null)
        {
            Debug.Log("no map");
        }
        else if (maps.gameObject.activeInHierarchy == false)
        {
            narrative.gameObject.SetActive(false);
            maps.gameObject.SetActive(true);

        }
        else
        {
            maps.gameObject.SetActive(false);
        }

    }
}
