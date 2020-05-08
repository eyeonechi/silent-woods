using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyMovement : MonoBehaviour {

    public Transform player;
    public Transform enemy;

    public float speed;

    public bool isOffScreen;

    public float offScreenDotRange = 0.7f;

   public bool isVisible = false;
    public float visibleDotRange = 0.8f;

    private bool isInRange = false;
    float followDistance;
    public float maxVisibleDistance = 25.0f;

    public float startingDistance;
    private float firstPaperDistance = 24.0f;



    public float collisionDistance;

    public AudioClip enemySightedSFX;
    public float health = 100;
    private bool hasPlayedSeenSound = false;
    public float reduceDistanceAmount;


    public float damage;
    GameObject staticObject;
    private float newAlpha;
    private object sceneManager;
    bool startMoving = false;

    float teleportDistance;
    float teleportRate;
    float dist = 1000;

	public void SaveState()
	{
		PlayerState.instance.enemyTeleportRate = teleportRate;
		PlayerState.instance.enemyTeleportDistance = teleportDistance;
	}

    // Use this for initialization
    void Start () {
		player = GameObject.Find("Player").transform;
        enemy = transform;

        followDistance = startingDistance;

		teleportRate = PlayerState.instance.enemyTeleportRate;
		teleportDistance = PlayerState.instance.enemyTeleportDistance;

        InvokeRepeating("TeleportEnemy", 1, teleportRate);

        staticObject = GameObject.Find("StaticObject");

	}

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("dist = " + dist);
       // Debug.Log("isVisible = " + isVisible);
        //  Debug.Log("startMoving " + startMoving);
         // Debug.Log("followDistance = " + followDistance);
         // Debug.Log("teleportDistance = " + teleportDistance);
         // Debug.Log("teleportRate = " + teleportRate);
        if (startMoving)
        {
            dist = (enemy.position - player.position).sqrMagnitude;

            if (dist > 1000)
            {
                RestoreHealth();
            }

            checkOffScreen();

            if (isOffScreen)
            {
                MoveEnemy();
                RestoreHealth();

                if (dist < 100)
                {

                    DeductHealth();
                    StopEnemy();


                    AudioSource.PlayClipAtPoint(enemySightedSFX, player.position, (1 - health / 100.0f) * 0.05f);
                }
            }
            else
            {
                CheckIfVisible();
                if (isVisible)
                {
                    DeductHealth();
                    StopEnemy();


                    AudioSource.PlayClipAtPoint(enemySightedSFX, player.position, (1 - health / 100.0f) * 0.05f);

                    

                }
                else
                {
                    // If far away then move, else stop
                    if (!isInRange)
                    {
                        MoveEnemy();
                    }
                    else
                    {
                        StopEnemy();
                    }
                }
            }
        }
       
    }


    void DeductHealth()
    {
        health -= (damage+10f) * Time.deltaTime;


        if (health <= 50) newAlpha = (1.0f - health / 100) * 0.5f;
        else newAlpha = 0.0f;
       
        
        Renderer rend = staticObject.GetComponent<Renderer>();
        Color col = rend.material.GetColor("_Color");
        col.a = newAlpha;
        rend.material.SetColor("_Color", col);
     

        if (health <= 0.0)
        {
            health = 0.0f;
            // Restart/End Game
            Cursor.visible = true;

			Destroy(GameObject.Find("PlayerState"));
            SceneManager.LoadScene("Ending");
        }
    }

    void RestoreHealth()
    {
        health += damage * Time.deltaTime;

        if (health <= 50) newAlpha = (1.0f - health / 100) * 0.5f;
        else newAlpha = 0.0f; 

        Renderer rend = staticObject.GetComponent<Renderer>();
        Color col = rend.material.GetColor("_Color");
        col.a = newAlpha;
        rend.material.SetColor("_Color", col);


        if (health >= 100.0f)
        {
            health = 100.0f;
        }
    }

    void checkOffScreen ()
    {
        Vector3 forward = player.forward.normalized;
        Vector3 other = (enemy.position - player.position).normalized;
        float product = Vector3.Dot(forward, other);
        if (product < offScreenDotRange)
        {
            isOffScreen = true;
        } else
        {
            isOffScreen = false;

        }
    }

    void MoveEnemy()
    {
        CheckDistance();
        // If not too close, move
        if (!isInRange)
        {
            // Maintain gravity
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);
            Vector3 dir = (player.position - enemy.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(enemy.position, enemy.forward, out hit, collisionDistance))
            {
                if (hit.collider.gameObject.name != "Player" && hit.collider.gameObject.name != "Terrain")
                {
                    dir += hit.normal * 50;
                }
            }
            Quaternion rot = Quaternion.LookRotation(dir);
            enemy.rotation = Quaternion.Slerp(enemy.rotation, rot, Time.deltaTime);
            enemy.position += enemy.forward * speed * Time.deltaTime;
        }
        else
        {
            StopEnemy();
        }
    }

    void StopEnemy()
    {
        transform.LookAt(player);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void CheckIfVisible()
    {
        Vector3 forward = player.forward.normalized;
        Vector3 other = (enemy.position - player.position).normalized;
        float product = Vector3.Dot(forward, other);

        

        if (product > visibleDotRange)
        {
            CheckMaxVisibleRange();
            if (isInRange)
            {
                // Linecast to check for occlusion
                RaycastHit hit;
                if (Physics.Raycast(player.position, player.transform.forward, out hit, 25f))
               // if (Physics.Linecast(enemy.position + /*(Vector3.up * 1.75f) +*/ enemy.forward, player.position, out hit))
                {
                    if (hit.collider.gameObject.name == "Enemy")
                    {
                        isVisible = true;
                    }
                }
            }
            else
            {
                isVisible = false;
            }
        }
        else
        {
            isVisible = false;
        }

        

    }


    

    void CheckDistance()
    {
        // To get the scalar and magnitude value
        float squareDistance = (enemy.position - player.position).sqrMagnitude;
        float squareFollowDistance = followDistance * followDistance;
        if (squareDistance < squareFollowDistance)
        {
            isInRange = true;
        } else
        {
            isInRange = false;
        }
    }


    public void ReduceDistance()
    {
        followDistance -= reduceDistanceAmount;
    }

    void CheckMaxVisibleRange()
    {
        float squareDistance  = (enemy.position - player.position).sqrMagnitude;
        float squareMaxDistance  = maxVisibleDistance * maxVisibleDistance;
        if (squareDistance < squareMaxDistance)
        {
            isInRange = true;
        }
        else
        {
            isInRange = false;
        }
    }

    public void SetFirstPaperDistance()
    {
        followDistance = firstPaperDistance;
    }


    public void TeleportEnemy()
    {
        // Check if out-of-view, then move
        checkOffScreen();
       
        // If off screen, check for teleport
        if (isOffScreen)
        {
            CheckDistance();
            // If not too close, teleport
            if (!isInRange)
            {
                // Determine a position to teleport to
                
                int randomPosition = 1;
               /* if (Random.Range(0, 2) == 1)
                {
                    randomPosition = 1;
                }*/
                Vector3 newPosition = player.position + (randomPosition * player.forward * teleportDistance);
                float yPos = newPosition.y;
                newPosition.y = 1000.0f;
                // Raycast to that position
                RaycastHit hit;
                if (Physics.Raycast(newPosition, -Vector3.up, out hit, 1000.0f))
                {
                    // Debug.Log("teleport");
                    // Check if hits the terrain
                    if (hit.collider.gameObject.name == "Terrain")
                    {
                        // Move the enemy to the new position
                        //enemy.position = hit.point + (Vector3.up * 0.5f);
                        newPosition.y = yPos;
                        enemy.position = newPosition + (Vector3.up * 0.5f);
                        enemy.LookAt(player);
                    }
                }
            }
        }
    }

    
    public void startMovement()
    {
        startMoving = true;
    }

    public void increaseTeleRate(int reduceTime)
    {
        CollectPaper paperScript = GameObject.Find("Papers").GetComponent<CollectPaper>();

        teleportRate = 10f - reduceTime;
        teleportDistance = 50f - reduceTime * 5;
        InvokeRepeating("TeleportEnemy", 1, teleportRate);
		if (paperScript.papers >= paperScript.papersToWin)
        {
            teleportDistance = 5f;

        }

    }

    void OnGUI()
    {
        int healthI = (int)(health);
        string healthS = healthI.ToString();
        GUI.Box(new Rect((Screen.width * 0.5f) - 60.0f, Screen.height - 35, 120, 25), "Health : " + healthS);
    }
}
