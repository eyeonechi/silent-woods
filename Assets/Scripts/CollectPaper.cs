
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CollectPaper : MonoBehaviour
{


    public int papers = 0;
    public int papersToWin;
    float distanceToPaper = 5f;
    EnemyMovement enemy;

    public AudioClip paperPickup;

    public Transform winScene;
    public GameObject Player;
    public GameObject mainCam;
    bool lastPaperChange = true;

    void Start()
    {
        // Find and store a reference to the enemy script
        // to reduce distance after each paper collected
        if (enemy == null)
        {
            GameObject enemyObject = GameObject.Find("Enemy");
            if (enemyObject)
            {
                enemy = enemyObject.GetComponent<EnemyMovement>();
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
                if (hit.collider.gameObject.name == "Paper")
                {
                    papers += 1;
                    Debug.Log("A paper was picked up. Total papers = " + papers);
                    AudioSource.PlayClipAtPoint(paperPickup, transform.position);
                    Destroy(hit.collider.gameObject);
                    // Make enemy follow closer
                    if (papers == 1)
                    {
                        enemy.SetFirstPaperDistance();
                    }
                    else
                    {
                        enemy.ReduceDistance();
                    }
                }
            }
        }
        
    }

    void OnGUI()
    {
        Color color = GUI.color;
        if (papers < papersToWin && lastPaperChange == true)
        {
           
            GUI.Box(new Rect((Screen.width * 0.5f) - 60, 10, 120, 25), "" + papers.ToString() + " Papers");
            color.a = Mathf.Lerp(1f, 0f, 3f);
            GUI.color = color;
            lastPaperChange = false;
        }
        
    }

    
}