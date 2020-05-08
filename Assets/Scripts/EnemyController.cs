using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    public ProjectileController projectilePrefab;
    public GameObject destroyExplosionPrefab;
    public PlayerController player;

    void Start ()
    {
        // Get player reference if none attached already
        if (this.player == null)
        {
            this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
    }

    // This should be hooked up to the health manager on this object
    public void DestroyMe()
    {
        // Create explosion effect
        GameObject explosion = Instantiate(this.destroyExplosionPrefab);
        explosion.transform.position = this.transform.position;

        // Destroy self
        Destroy(this.gameObject);
    }
    
    // Update is called once per frame
    void Update ()
    {
        HealthManager healthManager = this.gameObject.GetComponent<HealthManager>();
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Make enemy material darker based on its health
        renderer.material.color = Color.red * ((float)healthManager.GetHealth() / 100.0f);

        // Randomly fire a projectile
        if (Random.value < (0.0005f + (0.004f * GlobalOptions.difficulty)))
        {
            ProjectileController p = Instantiate<ProjectileController>(projectilePrefab);
            p.transform.position = this.transform.position;
            p.velocity = (this.player.transform.position - this.transform.position).normalized * 5.0f;
        }
	}
}
