
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CollectPaper : MonoBehaviour
{


    public int papers = 0;
    public int papersToWin = 4;
    float distanceToPaper = 5f;
    EnemyMovement enemy;

    public AudioClip paperPickup;

    public Transform winScene;
    public GameObject Player;
    public GameObject mainCam;
    bool paperTaken = false;

	bool[] papersCollected;

    void Start()
    {
		papers = PlayerState.instance.papers;
		papersCollected = PlayerState.instance.papersCollected;

		if (SceneManager.GetActiveScene ().name.Equals ("Game")) {
			// Destroy papers which have already been collected
			if (papersCollected [0] == true) {
				GameObject.Destroy (GameObject.Find ("Paper1"));
			}
			if (papersCollected [1] == true) {
				GameObject.Destroy (GameObject.Find ("Paper2"));
			}
			if (papersCollected [2] == true) {
				GameObject.Destroy (GameObject.Find ("Paper3"));
			}
			if (papersCollected [3] == true) {
				GameObject.Destroy (GameObject.Find ("Paper4"));
			}
		} else if (SceneManager.GetActiveScene ().name.Equals ("Inside")) {
			if (papersCollected [4] == true) {
				GameObject.Destroy (GameObject.Find ("Paper5"));
			}
		}

        // Find and store a reference to the enemy script
        // to reduce distance after each paper collected
        if (enemy == null)
        {
            GameObject enemyObject = GameObject.Find("Enemy");
            if (enemyObject)
            {
                enemy = enemyObject.GetComponent<EnemyMovement>();
                enemyObject.SetActive(false);
            }
           
        }
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distanceToPaper))
            {
                if (hit.collider.gameObject.tag == "Paper")
                {
					if (PlayerState.instance != null) {
						if (hit.collider.gameObject.name == "Paper1") {
							papersCollected [0] = true;
						} else if (hit.collider.gameObject.name == "Paper2") {
							papersCollected [1] = true;
						} else if (hit.collider.gameObject.name == "Paper3") {
							papersCollected [2] = true;
						} else if (hit.collider.gameObject.name == "Paper4") {
							papersCollected [3] = true;
						} else if (hit.collider.gameObject.name == "Paper5") {
							papersCollected [4] = true;
						}
					}
                    papers += 1;
                    Debug.Log("A paper was picked up. Total papers = " + papers);
					AudioSource.PlayClipAtPoint(paperPickup, hit.collider.transform.position);
                    Destroy(hit.collider.gameObject);
                    paperTaken = true;
                    // Make enemy follow closer
					if (enemy != null) {
						if (papers > 1) {

							enemy.ReduceDistance ();

							enemy.increaseTeleRate (papers * 2);

						}
						enemy.TeleportEnemy ();
					}


                    //enemy.SetFirstPaperDistance();




                }
            }
        }

		if (papers >= 1 && enemy != null)
        {
            enemy.startMovement();
            enemy.TeleportEnemy();
            enemy.gameObject.SetActive(true);
        }
        

    }

    void OnGUI()
    {
           
         GUI.Box(new Rect((Screen.width * 0.5f) - 60, 10, 120, 25), "" + papers.ToString() + " Tomes");
           
        
        
    }

	public void SaveState() {
		if (PlayerState.instance != null) {
			PlayerState.instance.papers = papers;
			PlayerState.instance.papersCollected = papersCollected;
		}
	}

    
}