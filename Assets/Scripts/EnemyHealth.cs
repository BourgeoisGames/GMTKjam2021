using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

	public float max_health;
    public float kill_score;
	private float health;

    public PlayerScore scorekeeper;

    // Start is called before the first frame update
    void Start()
    {
        health = max_health;
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
        scorekeeper.add_score(kill_score);
        Destroy(gameObject);
    }

}
