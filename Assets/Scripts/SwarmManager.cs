using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SwarmManager : MonoBehaviour {

    // External parameters/variables
    public GameObject enemyTemplate;
    public int enemyRows;
    public int enemyCols;
    public float enemySpacing;
    public float stepSize;
    public float stepTime;
    public float maxXDeviation;
    public UnityEvent swarmDestroyedEvent;
    public bool swarmDestroyed = true;

    // Internal parameters/variables
    private int direction;
    private float stepCountdown;
    private float swarmWidth;
    
	// Use this for initialization
	void Start () {
        // Initial parameters
        this.stepCountdown = this.stepTime;
        this.transform.localPosition = Vector3.left * maxXDeviation; // Start at far left
        this.direction = 1; // Start moving towards the right (positive x-axis)
	}
	
	// Update is called once per frame
	void Update () {
        this.stepCountdown -= Time.deltaTime;
        if (this.stepCountdown < 0.0f)
        {
            // Perform a single step to move the swarm across (or down)
            // Then reset the timer to periodically perform such steps
            this.StepSwarm();
            this.stepCountdown = this.stepTime;
        }

        // Check if swarm destroyed, and trigger event if so
        if (this.transform.childCount <= 0 && !this.swarmDestroyed)
        {
            this.swarmDestroyedEvent.Invoke();
            this.swarmDestroyed = true;
        }
	}

    // Method to automatically generate swarm of enemies based on the set public attributes
    public void GenerateSwarm()
    {
        // Create swarm of enemies in a grid formation
        for (int row = 0; row < enemyRows; row++)
        {
            for (int col = 0; col < enemyCols; col++)
            {
                GameObject enemy = GameObject.Instantiate<GameObject>(enemyTemplate);
                enemy.transform.parent = this.transform;
                enemy.transform.localPosition = new Vector3(col, 0.0f, row) * enemySpacing;
            }
        }
        this.swarmWidth = (enemyCols - 1) * enemySpacing;
        this.swarmDestroyed = false;
    }

    // Method to step a swarm across the screen (or down & reverse when it reaches the edge)
    private void StepSwarm()
    {
        // Check if the swarm has reached the "edge" of its allowed movement range, as specified
        // by the "Max X Deviation" parameter. If so swarm should move down; otherwise sideways.
        if (this.transform.localPosition.x < -maxXDeviation && this.direction == -1 || 
            this.transform.localPosition.x + swarmWidth > maxXDeviation && this.direction == 1)
        {
            // Move swarm down
            this.transform.Translate(Vector3.back * stepSize);
            this.direction = -this.direction;
        }
        else
        {
            // Move swarm sideways
            this.transform.Translate(Vector3.right * this.direction * stepSize);
        }
    }
}
