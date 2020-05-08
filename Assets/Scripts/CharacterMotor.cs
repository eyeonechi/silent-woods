// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class CharacterMotor : MonoBehaviour
{




    // Does this script currently respond to input?
    public bool canControl = true;

    public bool useFixedUpdate = true;

    // For the next variables, @System.NonSerialized tells Unity to not serialize the variable or show it in the inspector view.
    // Very handy for organization!

    // The current global direction we want the character to move in.
    
    public Vector3 inputMoveDirection = Vector3.zero;

    // Is the jump button held down? We use this interface instead of checking
    // for the jump button directly so this script can also be used by AIs.

    public bool inputJump = false;

    // The maximum horizontal speed when moving
    public float maxForwardSpeed = 10.0f;
    public float maxSidewaysSpeed = 10.0f;
    public float maxBackwardsSpeed = 10.0f;



    // How fast does the character change speeds?  Higher is faster.
    public float maxGroundAcceleration = 30.0f;
    public float maxAirAcceleration = 20.0f;

    // The gravity for the character
    public  float gravity = 10.0f;
    public float maxFallSpeed = 20.0f;


    // The last collision flags returned from controller.Move
    CollisionFlags collisionFlags; 

	// We will keep track of the character's current velocity,
    private Vector3 velocity;

    // This keeps track of our current velocity while we're not grounded

    public Vector3 hitPoint = Vector3.zero;

    public Vector3 lastHitPoint = new Vector3(Mathf.Infinity, 0, 0);

  
    // Can the character jump?
    bool enabled = true;

    // How high do we jump when pressing jump and letting go immediately
    public float baseHeight = 10.0f;

    // We add extraHeight units (meters) on top when holding the button down longer while jumping
    float extraHeight = 4.1f;

    // How much does the character jump out perpendicular to the surface on walkable surfaces?
    // 0 means a fully vertical jump and 1 means fully perpendicular.
    float perpAmount = 0.0f;

    // How much does the character jump out perpendicular to the surface on too steep surfaces?
    // 0 means a fully vertical jump and 1 means fully perpendicular.
    float steepPerpAmount = 0.5f;

    // For the next variables, @System.NonSerialized tells Unity to not serialize the variable or show it in the inspector view.
    // Very handy for organization!

    // Are we jumping? (Initiated with jump button and not grounded yet)
    // To see if we are just in the air (initiated by jumping OR falling) see the grounded variable.
    bool jumping = false;

    bool holdingJumpButton = false;

    // the time we jumped at (Used to determine for how long to apply extra jump power after jumping.)
    float lastStartTime = 0.0f;

    float lastButtonDownTime = -100;

    Vector3 jumpDir = Vector3.zero;

    bool grounded = true;
    Vector3 groundNormal = Vector3.zero;

    private Vector3 lastGroundNormal = Vector3.zero;

    private Transform tr;

    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        tr = transform;
    }

    private void UpdateFunction()
    {

        // Update velocity based on input
        if (grounded)
        velocity = ApplyInputVelocityChange(velocity);

        // Apply gravity and jumping force
        velocity = ApplyGravityAndJumping(velocity);

       
        // Save lastPosition for velocity calculation.
        Vector3 lastPosition = tr.position;

        // We always want the movement to be framerate independent.  Multiplying by Time.deltaTime does this.
        Vector3 currentMovementOffset = velocity * Time.deltaTime;

        // Find out how much we need to push towards the ground to avoid loosing grouning
        // when walking down a step or over a sharp change in slope.
        float pushDownOffset = Mathf.Max(controller.stepOffset, new Vector3(currentMovementOffset.x, 0, currentMovementOffset.z).magnitude);
        if (grounded)
            currentMovementOffset -= pushDownOffset * Vector3.up;

        // Reset variables that will be set by collision function
        
        groundNormal = Vector3.zero;

        // Move our character!
        collisionFlags = controller.Move(currentMovementOffset);

        lastHitPoint = hitPoint;
        lastGroundNormal = groundNormal;

      

        // Calculate the velocity based on the current and previous position.  
        // This means our velocity will only be the amount the character actually moved as a result of collisions.
        Vector3 oldHVelocity = new Vector3(velocity.x, 0, velocity.z);
        velocity = (tr.position - lastPosition) / Time.deltaTime;
        Vector3 newHVelocity = new Vector3(velocity.x, 0, velocity.z);

        // The CharacterController can be moved in unwanted directions when colliding with things.
        // We want to prevent this from influencing the recorded velocity.
        if (oldHVelocity == Vector3.zero)
        {
            velocity = new Vector3(0, velocity.y, 0);
        }
        else
        {
            float projectedNewVelocity = Vector3.Dot(newHVelocity, oldHVelocity) / oldHVelocity.sqrMagnitude;
            velocity = oldHVelocity * Mathf.Clamp01(projectedNewVelocity) + velocity.y * Vector3.up;
        }

        if (velocity.y < velocity.y - 0.001f)
        {
            if (velocity.y < 0)
            {
                // Something is forcing the CharacterController down faster than it should.
                // Ignore this
               
            }
            else
            {
                // The upwards movement of the CharacterController has been blocked.
                // This is treated like a ceiling collision - stop further jumping here.
                holdingJumpButton = false;
            }
        }

        // We were grounded but just loosed grounding
        if (grounded && !IsGroundedTest())
        {
            grounded = false;

          

            SendMessage("OnFall", SendMessageOptions.DontRequireReceiver);
            // We pushed the character down to ensure it would stay on the ground if there was any.
            // But there wasn't so now we cancel the downwards offset to make the fall smoother.
            tr.position += pushDownOffset * Vector3.up;
        }
        // We were not grounded but just landed on something
        else if (!grounded && IsGroundedTest())
        {
            grounded = true;
            jumping = false;
            

            SendMessage("OnLand", SendMessageOptions.DontRequireReceiver);
        }

      
    }

    void FixedUpdate()
    {
      

        if (useFixedUpdate)
            UpdateFunction();
    }

    void Update()
    {
        if (!useFixedUpdate)
            UpdateFunction();
    }

    private Vector3 ApplyInputVelocityChange(Vector3 velocity)
    {
        if (!canControl)
            inputMoveDirection = Vector3.zero;

        // Find desired velocity
        Vector3 desiredVelocity;
        if (grounded && TooSteep())
        {
            // The direction we're sliding in
            desiredVelocity = new Vector3(groundNormal.x, 0, groundNormal.z).normalized;
            // Find the input movement direction projected onto the sliding direction
            Vector3 projectedMoveDir = Vector3.Project(inputMoveDirection, desiredVelocity);
            // Add the sliding direction, the spped control, and the sideways control vectors
            desiredVelocity = desiredVelocity + projectedMoveDir + (inputMoveDirection - projectedMoveDir);
            // Multiply with the sliding speed
            
        }
        else
            desiredVelocity = GetDesiredHorizontalVelocity();

       

        if (grounded)
            desiredVelocity = AdjustGroundVelocityToNormal(desiredVelocity, groundNormal);
        else
            velocity.y = 0;

        // Enforce max velocity change
        float maxVelocityChange = GetMaxAcceleration(grounded) * Time.deltaTime;
        Vector3 velocityChangeVector = (desiredVelocity - velocity);
        if (velocityChangeVector.sqrMagnitude > maxVelocityChange * maxVelocityChange)
        {
            velocityChangeVector = velocityChangeVector.normalized * maxVelocityChange;
        }
        // If we're in the air and don't have control, don't apply any velocity change at all.
        // If we're on the ground and don't have control we do apply it - it will correspond to friction.
        if (grounded || canControl)
            velocity += velocityChangeVector;

        if (grounded)
        {
            // When going uphill, the CharacterController will automatically move up by the needed amount.
            // Not moving it upwards manually prevent risk of lifting off from the ground.
            // When going downhill, DO move down manually, as gravity is not enough on steep hills.
            velocity.y = Mathf.Min(velocity.y, 0);
        }

        return velocity;
    }

    private Vector3 ApplyGravityAndJumping(Vector3 velocity)
    {

        if (!inputJump || !canControl)
        {
            holdingJumpButton = false;
            lastButtonDownTime = -100;
        }

        if (inputJump && lastButtonDownTime < 0 && canControl)
            lastButtonDownTime = Time.time;

        if (grounded)
            velocity.y = Mathf.Min(0, velocity.y) - gravity * Time.deltaTime;
        else
        {
            velocity.y = velocity.y - gravity * Time.deltaTime;

            // When jumping up we don't apply gravity for some time when the user is holding the jump button.
            // This gives more control over jump height by pressing the button longer.
            if (jumping && holdingJumpButton)
            {
                // Calculate the duration that the extra jump force should have effect.
                // If we're still less than that duration after the jumping time, apply the force.
                if (Time.time < lastStartTime + extraHeight / CalculateJumpVerticalSpeed(baseHeight))
                {
                    // Negate the gravity we just applied, except we push in jumpDir rather than jump upwards.
                    velocity += jumpDir * gravity * Time.deltaTime;
                }
            }

            // Make sure we don't fall any faster than maxFallSpeed. This gives our character a terminal velocity.
            velocity.y = Mathf.Max(velocity.y, -maxFallSpeed);
        }

        if (grounded)
        {
            // Jump only if the jump button was pressed down in the last 0.2f seconds.
            // We use this check instead of checking if it's pressed down right now
            // because players will often try to jump in the exact moment when hitting the ground after a jump
            // and if they hit the button a fraction of a second too soon and no new jump happens as a consequence,
            // it's confusing and it feels like the game is buggy.
            if (enabled && canControl && (Time.time - lastButtonDownTime < 0.2f))
            {
                grounded = false;
                jumping = true;
                lastStartTime = Time.time;
                lastButtonDownTime = -100;
                holdingJumpButton = true;

                // Calculate the jumping direction
                if (TooSteep())
                    jumpDir = Vector3.Slerp(Vector3.up, groundNormal, steepPerpAmount);
                else
                    jumpDir = Vector3.Slerp(Vector3.up, groundNormal, perpAmount);

                // Apply the jumping force to the velocity. Cancel any vertical velocity first.
                velocity.y = 0;
                velocity += jumpDir * CalculateJumpVerticalSpeed(baseHeight);

               

                SendMessage("OnJump", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                holdingJumpButton = false;
            }
        }

        return velocity;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0 && hit.normal.y > groundNormal.y && hit.moveDirection.y < 0)
        {
            if ((hit.point - lastHitPoint).sqrMagnitude > 0.001f || lastGroundNormal == Vector3.zero)
                groundNormal = hit.normal;
            else
                groundNormal = lastGroundNormal;

            hitPoint = hit.point;
        }
    }



    private Vector3 GetDesiredHorizontalVelocity()
    {
        // Find desired velocity
        Vector3 desiredLocalDirection = tr.InverseTransformDirection(inputMoveDirection);
        float maxSpeed = MaxSpeedInDirection(desiredLocalDirection);
        if (grounded)
        {
            // Modify max speed on slopes based on slope speed multiplier curve
            float movementSlopeAngle = Mathf.Asin(velocity.normalized.y) * Mathf.Rad2Deg;
        }
        return tr.TransformDirection(desiredLocalDirection * maxSpeed);
    }

    private Vector3 AdjustGroundVelocityToNormal(Vector3 hVelocity, Vector3 groundNormal)
    {
        Vector3 sideways = Vector3.Cross(Vector3.up, hVelocity);
        return Vector3.Cross(sideways, groundNormal).normalized * hVelocity.magnitude;
    }

    private bool IsGroundedTest()
    {
        return (groundNormal.y > 0.01f);
    }

    float GetMaxAcceleration(bool grounded)
    {
        // Maximum acceleration on ground and in air
        if (grounded)
            return maxGroundAcceleration;
        else
            return maxAirAcceleration;
    }

    float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * targetJumpHeight * gravity);
    }

    bool IsJumping()
    {
        return jumping;
    }

   

    bool IsTouchingCeiling()
    {
        return (collisionFlags & CollisionFlags.CollidedAbove) != 0;
    }

    bool IsGrounded()
    {
        return grounded;
    }

    bool TooSteep()
    {
        return (groundNormal.y <= Mathf.Cos(controller.slopeLimit * Mathf.Deg2Rad));
    }

    Vector3 GetDirection()
    {
        return inputMoveDirection;
    }

    void SetControllable(bool controllable)
    {
        canControl = controllable;
    }

    // Project a direction onto elliptical quater segments based on forward, sideways, and backwards speed.
    // The function returns the length of the resulting vector.
    float MaxSpeedInDirection(Vector3 desiredMovementDirection)
    {
        if (desiredMovementDirection == Vector3.zero)
            return 0;
        else
        {
            float zAxisEllipseMultiplier = (desiredMovementDirection.z > 0 ? maxForwardSpeed : maxBackwardsSpeed) / maxSidewaysSpeed;
            Vector3 temp = new Vector3(desiredMovementDirection.x, 0, desiredMovementDirection.z / zAxisEllipseMultiplier).normalized;
            float length = new Vector3(temp.x, 0, temp.z * zAxisEllipseMultiplier).magnitude * maxSidewaysSpeed;
            return length;
        }
    }

  

    // Require a character controller to be attached to the same game object
    

}