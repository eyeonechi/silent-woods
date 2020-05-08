#pragma strict
@script RequireComponent(AudioSource)

var papers : int = 0;
var papersToWin : int;
var distanceToPaper : float = 5;
var enemy : EnemyScript;

public var paperPickup : AudioClip;

function Start () {
			// Find and store a reference to the enemy script
	// to reduce distance after each paper collected
	if (enemy == null) {
		var enemyObject : GameObject = GameObject.Find("Enemy");
		if (enemyObject) {
			enemy = enemyObject.GetComponent(EnemyScript);
		}
	}
}

function Update () {
	if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) {
		var ray = Camera.main.ScreenPointToRay(Vector3(Screen.width * 0.5, Screen.height * 0.5, 0.0));
		var hit : RaycastHit;
		if (Physics.Raycast(ray, hit, distanceToPaper)) {
			print(hit.collider.gameObject.name);
			if (hit.collider.gameObject.name == "Paper") {
				papers += 1;
				Debug.Log("A paper was picked up. Total papers = " + papers);
				GetComponent.<AudioSource>().PlayClipAtPoint(paperPickup, transform.position);
				Destroy(hit.collider.gameObject);
				// Make enemy follow closer
				if (papers == 1) {
					enemy.SetFirstPaperDistance();
				} else {
					enemy.ReduceDistance();
				}
			}
		}
	}
	if (papers == papersToWin) {
		SceneManagement.SceneManager.LoadScene("Win");
	}
}

function OnGUI() {
	if (papers < papersToWin) {
		GUI.Box(Rect((Screen.width * 0.5) - 60, 10, 120, 25), "" + papers.ToString() + " Papers");
	}
	/* else {
		//GUI.Box(Rect((Screen.width / 2) - 100, 10, 200, 35), "All Papers Collected!");
		//SceneManagement.InGameController.lastGameWon = true;
		SceneManagement.SceneManager.LoadScene("Win");
	}
	*/
}
