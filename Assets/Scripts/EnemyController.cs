using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

	public float move_speed;
	public Rigidbody body;
	public Pathfinding pathfinding;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        do_move();
    }

    void do_move()
    {
    	Vector3 to_move = get_move_amount();

    	body.velocity = to_move;
    }

    Vector3 get_move_amount()
    {
    	Vector3 direction = pathfinding.get_direction();
    	direction.y = 0.0f;
    	Vector3 to_move = Vector3.zero;
    	to_move += direction.normalized * move_speed;

    	return to_move;
    }

}
