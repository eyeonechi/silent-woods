#pragma strict
@script RequireComponent(AudioSource)


public var player : Transform;
public var enemy : Transform;

public var speed : float = 5.0;

var isOffScreen : boolean = false;
public var offScreenDotRange : float = 0.7;

var isVisible : boolean = false;
public var visibleDotRange : float = 0.8;

var isInRange : boolean = false;

public var followDistance : float = 24.0;
public var maxVisibleDistance : float = 25.0;

public var reduceDistanceAmount : float = 3.1;

public var startingDistance : float = 2000.0;
private var firstPaperDistance : float = 24.0;

private var squareDistance : float = 0.0;

public var health : float = 100.0;
public var damage : float = 20.0;

public var enemySightedSFX : AudioClip;

private var hasPlayedSeenSound : boolean = false;

private var collisionDistance : float = 5.0;

public var staticObject: GameObject;
private var volume : float;

function Start () {
	if (player == null) {
		//player = GameObject.Find("Player").transform;
	}
	enemy = transform;
	firstPaperDistance = followDistance;
	followDistance = startingDistance;
	InvokeRepeating("TeleportEnemy", 1, 10);

	var staticObject : GameObject = GameObject.Find("StaticObject");
	
}

function Update () {
	// Check if out-of-view, then move
	//GetComponent.<AudioSource>().PlayClipAtPoint(enemySightedSFX, player.position, 0.5 - health/100/2);

	CheckIfOffScreen();
	if (isOffScreen) {
		MoveEnemy();
		RestoreHealth();
	} else {
		// Check if player is seen
		CheckIfVisible();
		if (isVisible) {
			DeductHealth();
			StopEnemy();
			// Play sound only when the enemy is first sighted
			if (!hasPlayedSeenSound) {
				GetComponent.<AudioSource>().PlayClipAtPoint(enemySightedSFX, player.position, 0.5 - health/100/2);

			}
			hasPlayedSeenSound = true;
		} else {
			CheckMaxVisibleRange();
			// If far away then move, else stop
			if (!isInRange) {
				MoveEnemy();
			} else {
				StopEnemy();
			}
			// Reset hasPlayedSeenSound for next time isVisible first occurs
			hasPlayedSeenSound = false;
		}
	}
}

function DeductHealth() {
	health -= damage * Time.deltaTime;
	if (health <= 50.0) {
		var newAlpha : float = (1.0 - health/100);
	}
	staticObject.GetComponent.<Renderer>().material.color.a = newAlpha;

	if (health <= 0.0) {
		health = 0.0;
		// Restart/End Game
		Cursor.visible = true;
		Destroy(gameObject.Find("Player"));
		SceneManagement.SceneManager.LoadScene("Ending");
	}
}

function RestoreHealth() {
	health += damage * Time.deltaTime;
	if (health <= 50.0) {
		var newAlpha : float = (1.0 - health/100);
	}
	staticObject.GetComponent.<Renderer>().material.color.a = newAlpha;
	if (health >= 100.0) {
		health = 100.0;
	}
}

function CheckIfOffScreen() {
	var forward : Vector3 = player.forward.normalized;
	var other : Vector3 = (enemy.position - player.position).normalized;
	var product : float = Vector3.Dot(forward, other);
	if (product < offScreenDotRange) {
		isOffScreen = true;
	} else {
		isOffScreen = false;
	}
}

function MoveEnemy() {
	CheckDistance();
	// If not too close, move
	if (!isInRange) {
		// Maintain gravity
		GetComponent.<Rigidbody>().velocity = Vector3(0, GetComponent.<Rigidbody>().velocity.y, 0);
		var dir : Vector3 = (player.position - enemy.position).normalized;
		var hit : RaycastHit;
		if (Physics.Raycast(enemy.position, enemy.forward, hit, collisionDistance)) {
			if (hit.collider.gameObject.name != "Player" && hit.collider.gameObject.name != "Terrain") {
				dir += hit.normal * 50;
			}
		}
		var rot : Quaternion = Quaternion.LookRotation(dir);
		enemy.rotation = Quaternion.Slerp(enemy.rotation, rot, Time.deltaTime);
		enemy.position += enemy.forward * speed * Time.deltaTime;
	} else {
		StopEnemy();
	}
}

function StopEnemy() {
	transform.LookAt(player);
	GetComponent.<Rigidbody>().velocity = Vector3.zero;
}

function CheckIfVisible() {
	var forward : Vector3 = player.forward.normalized;
	var other : Vector3 = (enemy.position - player.position).normalized;
	var product : float = Vector3.Dot(forward, other);
	if (product > visibleDotRange) {
		CheckMaxVisibleRange();
		if (isInRange) {
			// Linecast to check for occlusion
			var hit : RaycastHit;
			if (Physics.Linecast(enemy.position + (Vector3.up * 1.75) + enemy.forward, player.position, hit)) {
				if (hit.collider.gameObject.name == "Player") {
					isVisible = true;
				}
			}
		} else {
			isVisible = false;
		}
	} else {
		isVisible = false;
	}
}

function CheckDistance() {
	var squareDistance : float = (enemy.position - player.position).sqrMagnitude;
	var squareFollowDistance : float = followDistance * followDistance;
	if (squareDistance < squareFollowDistance) {
		isInRange = true;
	} else {
		isInRange = false;
	}
}

function ReduceDistance() {
	followDistance -= reduceDistanceAmount;
}

function CheckMaxVisibleRange() {
	var squareDistance : float = (enemy.position - player.position).sqrMagnitude;
	var squareMaxDistance : float = maxVisibleDistance * maxVisibleDistance;
	if (squareDistance < squareMaxDistance) {
		isInRange = true;
	} else {
		isInRange = false;
	}
}

function SetFirstPaperDistance() {
	followDistance = firstPaperDistance;
}

function TeleportEnemy() {
	// Check if out-of-view, then move
	CheckIfOffScreen();
	// If off screen, check for teleport
	if (isOffScreen) { 
		CheckDistance();
		// If not too close, teleport
		if (!isInRange) {
			// Determine a position to teleport to
			var teleportDistance : float = 50.0;
			var randomPosition : int = -1;
			if (Random.Range(0, 2) == 1) {
				randomPosition = 1;
			}
			var newPosition : Vector3 = player.position + (randomPosition * player.right * teleportDistance);
			newPosition.y = 1000.0;
			// Raycast to that position
			var hit : RaycastHit;
			if (Physics.Raycast(newPosition, -Vector3.up, hit, 1000.0)) {
				// Check if hits the terrain
				if (hit.collider.gameObject.name == "Terrain") {
					// Move the enemy to the new position
					enemy.position = hit.point + (Vector3.up * 0.5);
					enemy.LookAt(player);
				}
			}
		}
	}
}

function OnGUI() {
	GUI.Box(Rect((Screen.width * 0.5) - 60, Screen.height - 35, 120, 25), "Health : " + parseInt(health).ToString());
}
