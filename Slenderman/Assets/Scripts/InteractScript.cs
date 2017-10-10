using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InteractScript : MonoBehaviour {

	public float interactDistance = 5f;
	public AudioClip keyPickup;
	private Vector3 position;
	static bool created = false;

	void Awake()
	{
		Scene currentScene = SceneManager.GetActiveScene ();
		Debug.Log (currentScene.name);
		Debug.Log (created && currentScene.name.Equals ("Game"));

		if (!created) {
			DontDestroyOnLoad (transform.parent);
			DontDestroyOnLoad (GameObject.Find ("Terrain"));
			DontDestroyOnLoad (GameObject.Find ("Building"));
			DontDestroyOnLoad (GameObject.Find ("Plane"));
			created = true;
		} else if (created && currentScene.name.Equals("Game")){
			//Destroy (transform.parent.gameObject);
		}
		Debug.Log (created);
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, interactDistance))
			{
				if (hit.collider.CompareTag("HouseScene"))
				{
					position = transform.parent.position;
					position.y = 6;
					transform.parent.position = position;
					position = transform.parent.position;
					//Destroy (this.gameObject);
					SceneManager.LoadScene ("Inside");
				}
				else if(hit.collider.CompareTag("Door"))
				{
					DoorScript doorScript = hit.collider.transform.parent.GetComponent<DoorScript>();
					if(doorScript == null) return;
					
					if(doorScript.name == "hinge")
					{
						doorScript.ChangeDoorState();
						return;
					}
					
					if(Inventory.keys[doorScript.index] == true)
					{
						doorScript.ChangeDoorState();
					}
				}
				else if(hit.collider.CompareTag("Key"))
				{
					Inventory.keys[hit.collider.GetComponent<Key>().index] = true;
					AudioSource.PlayClipAtPoint(keyPickup, hit.collider.transform.position);
					Destroy(hit.collider.gameObject);
				}
				else if(hit.collider.CompareTag("NextLeveldoor"))
				{
					position = transform.parent.position;
					position.y = 6;
					transform.parent.position = position;
					//Destroy (this.gameObject);
					SceneManager.LoadScene ("Game");
				}
			}
			
		}
	}
}
