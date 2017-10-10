using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InteractScript : MonoBehaviour {

	public float interactDistance = 5f;
	public AudioClip keyPickup;

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit, interactDistance))
			{
				if(hit.collider.CompareTag("Door"))
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
					SceneManager.LoadScene ("Outside");
				}
			}
			
		}
	}
}
