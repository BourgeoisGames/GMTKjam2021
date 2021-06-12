using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

	public float move_speed;
	public float speed_variation_range = 0.0f;
	public Rigidbody body;
	public Pathfinding pathfinding;

	public float attack_speed;
	private float attack_timer;
	private bool can_attack;

	public float attack_damage;
	public float attack_range;

	public PlayerController target;

	public EnemyHealth enemy_health;

	public PlayerScore scorekeeper;

	public EnemyAnimation enemy_animation;

    public AudioSource oneShotSource;
    public AudioClip attackPlayerClip;

    public Transform enemy_transform
    {
        get { return transform; }
    }


    // Start is called before the first frame update
    void Start()
    {
        can_attack = true;
  		attack_timer = 0.0f;
  		pathfinding.target = target;
  		move_speed = move_speed + Random.value*2*speed_variation_range - speed_variation_range;
    }

    public void set_target(PlayerController targ)
    {
    	target = targ;
    	pathfinding.target = targ;
    }

    public void set_scorekeeper(PlayerScore sk)
    {
    	scorekeeper = sk;
    	enemy_health.scorekeeper = sk;
    }

    // Update is called once per frame
    void Update()
    {
        handle_attack();
        handle_attack_animation();
    }

    void handle_attack_animation()
    {
    	if(attack_position_is_valid()){
        	enemy_animation.set_attacking(true);
		}
		else{
			enemy_animation.set_attacking(false);
		}
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

    	handle_rotation(to_move);
    }

    void handle_rotation(Vector3 to_move)
    {
    	if (to_move != Vector3.zero) {
    		transform.rotation = Quaternion.LookRotation(to_move);
		}
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
    		do_attack();
    	}
    }

    bool attack_position_is_valid()
    {
    	return Vector3.Distance(target.transform.position, transform.position) <= attack_range;
    }

    void do_attack()
    {
    	handle_attack_cooldown();
    	target.player_health.take_damage(attack_damage);
        oneShotSource.PlayOneShot(attackPlayerClip);
    }


}
