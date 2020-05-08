using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyMovement : MonoBehaviour {

    public Transform player;
    public Transform enemy;

    public float speed;

    private bool isOffScreen;

    public float offScreenDotRange = 0.7f;

    private bool isVisible = false;
    public float visibleDotRange = 0.8f;

    private bool isInRange = false;
    public float followDistance = 24.0f;
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


    // Use this for initialization
    void Start () {
		if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }
        enemy = transform;
        followDistance = startingDistance;
        InvokeRepeating("TeleportEnemy", 1, 10);

        staticObject = GameObject.Find("StaticObject");
	}

    // Update is called once per frame
    void Update()
    {

        

        checkOffScreen();
        if (isOffScreen)
        {
            MoveEnemy();
            RestoreHealth();
        }
        else
        {
            CheckIfVisible();
            if (isVisible)
            {
                DeductHealth();
                StopEnemy();

               
                AudioSource.PlayClipAtPoint(enemySightedSFX, player.position, (1 - health / 100.0f)*0.5f);
                
                hasPlayedSeenSound = true;

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
                // Reset hasPlayedSeenSound for next time isVisible first occurs
                hasPlayedSeenSound = false;
            }
        }
    }


    void DeductHealth()
    {
        health -= damage * Time.deltaTime;

       

        newAlpha = (1.0f - health / 100) * 0.5f;
        
        Renderer rend = staticObject.GetComponent<Renderer>();
        Color col = rend.material.GetColor("_Color");
        col.a = newAlpha;
        rend.material.SetColor("_Color", col);
     

        if (health <= 0.0)
        {
            health = 0.0f;
            // Restart/End Game
            Cursor.visible = true;
            Destroy(GameObject.Find("Player"));
            SceneManager.LoadScene("Ending");
        }
    }

    void RestoreHealth()
    {
        health += damage * Time.deltaTime;

        newAlpha = (1.0f - health / 100) * 0.5f;
        
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
                if (Physics.Linecast(enemy.position + (Vector3.up * 1.75f) + enemy.forward, player.position, out hit))
                {
                    if (hit.collider.gameObject.name == "Player")
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


    void TeleportEnemy()
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
                float teleportDistance = 50.0f;
                int randomPosition = -1;
                if (Random.Range(0, 2) == 1)
                {
                    randomPosition = 1;
                }
                Vector3 newPosition = player.position + (randomPosition * player.right * teleportDistance);
                newPosition.y = 1000.0f;
                // Raycast to that position
                RaycastHit hit;
                if (Physics.Raycast(newPosition, -Vector3.up, out hit, 1000.0f))
                {
                    // Check if hits the terrain
                    if (hit.collider.gameObject.name == "Terrain")
                    {
                        // Move the enemy to the new position
                        enemy.position = hit.point + (Vector3.up * 0.5f);
                        enemy.LookAt(player);
                    }
                }
            }
        }
    }

    

    void OnGUI()
    {
        int healthI = (int)(health);
        string healthS = healthI.ToString();
        GUI.Box(new Rect((Screen.width * 0.5f) - 60.0f, Screen.height - 35, 120, 25), "Health : " + healthS);
    }
}
