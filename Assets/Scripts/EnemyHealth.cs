using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

	public float max_health;
    public float kill_score;
	private float health;

    public float health_variation_range;

    public EnemyController controller;

    public PlayerScore scorekeeper;

    public GameObject deathPrefab;

    //public Transform enemy_transform;

    // Start is called before the first frame update
    void Start()
    {
        float start_health = max_health + Random.value*2*health_variation_range - health_variation_range;
        health = start_health;
        Vector3 new_scale = transform.localScale;
        new_scale.y = transform.localScale.y * (Mathf.Max(Mathf.Sqrt(start_health/max_health), 1.0f));
        transform.localScale = new_scale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void take_damage(float to_take)
    {
        health -= to_take;
        if(health <= 0.0f)
        {
            health = 0.0f;
        }

        check_for_death();
    }

    public float get_health()
    {
        return health;
    }

    void check_for_death()
    {
        if(health <= 0.0f)
        {
            die();
        }
    }

    void die()
    {
        Instantiate(deathPrefab, controller.enemy_transform.position, controller.enemy_transform.rotation);
        scorekeeper.add_score(kill_score);
        Destroy(gameObject);
    }

}
