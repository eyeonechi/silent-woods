/*
#pragma strict
@script RequireComponent {
	Rigidbody
}

private var myTransform : Transform;
private var myRigidbody : Rigidbody;

var target : Transform;
var moveSpeed : float = 6.0;
var turnSpeed : float = 2.0;

private var desiredVelocity : Vector 3;

var isGrounded : boolean = false;
var rayDistance : float = 5.0;

enum NPC {
	Idle,
	FreeRoam,
	Chasing,
	RunningAway
}

var state : NPC;

var minimumRange : float = 5.0;
var maximumRange : float = 45.0;

private var minimumRangeSqr : float;
private var maximumRangeSqr : float;

var isNpcChasing : boolean = true;

var freeRoamTimer : float = 0.0;
var freeRoamTimerMax : float = 5.0;
var freeRoamTimerMaxRange : float = 1.5;
var freeRoamTimerMaxRangeAdjusted : float = 5.0;

var calcDir : Vector3;

var isSlender : boolean = true;

var isVisible : boolean = false;

var offScreenDot : float = 0.8;

function Start () {
	minimumRangeSqr = minimumRange * minimumRange
	maximumRangeSqr = maximumRange * maximumRange

	myTransform = transform;
	myRigidboy = rigidbody;

	myRigidbody.freezeRotation = true;

	freeRoamTimer = 1000.0;

	if (isSlender) {
		InvokeRepeating("TeleportEnemy", 60.0, 20.0);
	}
}

function TeleportEnemy) {
	CheckIfVisible();
	if (!isVisible) {
		var sqrDist : float = (target.position - myTransform.position).sqrMagnitude;
		if (sqrDist > maximumRangeSqr + 25.0) {
			var teleportDistance : float = maximumRange + 15.0;
			var rndDir : int = Random.Range(0,2);

			if (rndDir == 0) {
				rndDir = -1;
			}
			var terrainPosCheck : Vector3 = target.position + (rndDir * target.right * teleportDistance);
		}
	}
}

function Update () {
	
}
*/