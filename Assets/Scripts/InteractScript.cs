using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InteractScript : MonoBehaviour {

	public float interactDistance = 5f;
	public AudioClip keyPickup;
	private Vector3 position;
	static bool created = false;
	public CollectPaper collectPaper;
	public Inventory inventory;
    public GameObject papers;

	void Start ()
	{
		if (SceneManager.GetActiveScene().name.Equals("Game"))
		{
			Vector3 position = new Vector3 (PlayerState.instance.x, PlayerState.instance.y, PlayerState.instance.z);
			transform.position = position;
		}
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
					if (SceneManager.GetActiveScene().name.Equals("Game")) {
						SaveState ();
					}
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

					pauseController notDoor = GameObject.Find ("PauseMenuController").GetComponent<pauseController> ();

					if (inventory != null && inventory.keys [doorScript.index] == true) {
						doorScript.ChangeDoorState ();
					} else if (doorScript.index == 3) {
						notDoor.BadDoorNot ();
					} else {
						notDoor.GoodDoorNot ();
					}
				}
				else if(hit.collider.CompareTag("Key"))
				{
					if (inventory != null) {
						if (hit.collider.gameObject.name == "Key1") {
							inventory.keys [1] = true;
						} else if (hit.collider.gameObject.name == "Key2") {
							inventory.keys [2] = true;
						} else if (hit.collider.gameObject.name == "Key3") {
							inventory.keys [3] = true;
						}
					}
					AudioSource.PlayClipAtPoint(keyPickup, hit.collider.transform.position);
					Destroy(hit.collider.gameObject);
				}
				else if(hit.collider.CompareTag("NextLeveldoor"))
				{
					if (SceneManager.GetActiveScene().name.Equals("Inside")) {
						SaveState ();
					}
					SceneManager.LoadScene ("Game");
				}


                else if(hit.collider.CompareTag("Rock"))
                {
					Debug.Log (hit.collider.name);
                    if (papers)
                    {
						Debug.Log ("in paper");

                        CollectPaper paperScript = papers.GetComponent<CollectPaper>();
						if (paperScript.papers >= paperScript.papersToWin) {
							Destroy(GameObject.Find("PlayerState"));
							SceneManager.LoadScene ("Win");
						}
                    }

                    
                }
			}
			
		}
	}

	public void SaveState() {
		if (PlayerState.instance != null && SceneManager.GetActiveScene().name.Equals("Game")) {
			PlayerState.instance.x = transform.position.x;
			PlayerState.instance.y = transform.position.y;
			PlayerState.instance.z = transform.position.z;
		}
		inventory.SaveState ();
		collectPaper.SaveState ();
	}

}
