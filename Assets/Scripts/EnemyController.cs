using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

	public float move_speed;
	public Rigidbody body;
	public Pathfinding pathfinding;

	public float attack_speed;
	private float attack_timer;
	private bool can_attack;

	public float attack_damage;
	public float attack_range;

	public GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        can_attack = true;
  		attack_timer = 0.0f;
  		pathfinding.target = target;
    }

    public void set_target(GameObject targ)
    {
    	target = targ;
    }

    // Update is called once per frame
    void Update()
    {
        handle_attack();
    }

    void update_attack_availability()
    {
    	if(attack_timer < Time.time){
    		can_attack = true;
    	}
    }

    void handle_attack_cooldown()
    {
    	can_attack = false;
    	attack_timer = Time.time + attack_speed;
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

    void handle_attack()
    {
    	update_attack_availability();
    	if(! can_attack){
    		return;
    	}

    	if(attack_position_is_valid()){
    		handle_attack_cooldown();
    		do_attack();
    	}
    }

    bool attack_position_is_valid()
    {
    	return false;//if()
    }

    void do_attack()
    {
    	return;
    }


}
