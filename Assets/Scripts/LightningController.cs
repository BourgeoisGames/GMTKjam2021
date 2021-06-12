using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
	
	public GameObject prefab;

	public GameObject lightning_animation_prefab;

    public AudioSource lightningSource;
    public AudioClip placePylon, placeLightning;

    public AudioSource zapSource;

	private LightningAnimation _active_animation;

	public float damage_over_time = 2;
	public float max_lightning_range = 100000000.0f;

	private bool _lightning_is_active;
	
	private string player_tag = "PlayerCharacter";
	private string enemy_tag = "Enemy";
	private string wall_tag = "Wall";

	private float raycast_ignore_distance = 0.01f;

	private List<LightningEndpoint> _lighting_points;
	
	void Start() {
		_lighting_points = new List<LightningEndpoint>();
		_lightning_is_active = false;
		_active_animation = null;
	}

	void Update() {
		if(_lightning_is_active){
			handle_active_lightning_pair(_lighting_points[0], _lighting_points[1]);
		}
	}
	
	public void activate(RaycastHit target_hit) {

		if(target_hit.collider.gameObject.tag != wall_tag){
			return; //we've got to hit a wall;
		}

		Vector3 target = target_hit.point;
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, target_hit.normal);
		if (_lighting_points.Count >= 2) {
			remove_old_ball();
		}
		spawn_ball(target, targetRotation);
		if (_lighting_points.Count == 2) {
			start_lightning_effect();
		}

        // Play the appropriate sound effect
        if (_lightning_is_active)
        {
            lightningSource.PlayOneShot(placeLightning);
        }
        else
        {
            lightningSource.PlayOneShot(placePylon);
        }
    }
	
	private void remove_old_ball() {
		/* removes and cleans up the oldest lightning ball in the list */
		if (_active_animation != null) {
			_active_animation.despawn();
		}
		if (_lighting_points.Count != 0) {
			_lighting_points[0].despawn();
			_lighting_points.RemoveAt(0);
		}
	}
	
	private void start_lightning_effect() {
		_lightning_is_active = true;
		handle_lightning_animation(_lighting_points[0].get_lightning_position(), _lighting_points[1].get_lightning_position());

        // Perform an initial check to see if the lightning is valid
        handle_active_lightning_pair(_lighting_points[0], _lighting_points[1]);
    }

	private void end_lightning_effect() {
		_lightning_is_active = false;
		if(_active_animation != null){
			_active_animation.despawn();
		}
	}
	
	public void reset_balls() {
		foreach (LightningEndpoint ball in _lighting_points) {
			ball.despawn();
		}
		_lighting_points = new List<LightningEndpoint>();
		end_lightning_effect();
	}
	
	private void spawn_ball(Vector3 target, Quaternion targetRotation) {
		GameObject new_ball = Instantiate(prefab) as GameObject;
		new_ball.transform.position = target;
        new_ball.transform.rotation = targetRotation;
		_lighting_points.Add(getLightningBallComponent(new_ball));
	}
	
	private LightningEndpoint getLightningBallComponent(GameObject lightning_ball) {
		LightningEndpoint comp = lightning_ball.GetComponent<LightningEndpoint>();;
		if (comp == null) {
			Debug.Log("Lightning Ball prefab is missing the LightningEndpoint script");
		} 
		return comp;
	}
	
	void handle_active_lightning_pair(LightningEndpoint lightning_point_a, LightningEndpoint lightning_point_b)
	{
		Vector3 point_a = lightning_point_a.get_lightning_position();
        Vector3 point_b = lightning_point_b.get_lightning_position();

        Vector3 direction = point_b - point_a;
		direction = direction.normalized;

		float distance = Vector3.Distance(point_a, point_b);

		if(!is_valid_lightning_distance(distance)){
			handle_invalid_lightning();
		}

		RaycastHit[] hit_list = Physics.RaycastAll(point_a, direction, distance);
		RaycastHit[] hit_list_b_to_a = Physics.RaycastAll(point_b, -direction, distance);

		if(has_invalid_lightning_hits(hit_list, point_a, point_b) || has_invalid_lightning_hits(hit_list_b_to_a, point_b, point_a)){
			handle_invalid_lightning();
		}
		handle_lightning_damage_list(hit_list);
	}

	bool is_valid_lightning_distance(float distance)
	{
		return distance <= max_lightning_range;
	}

	bool has_invalid_lightning_hits(RaycastHit[] hit_list, Vector3 point_a, Vector3 point_b)
	{
		foreach (RaycastHit hit in hit_list) {
			if(hit.collider.gameObject.tag == wall_tag){
				Vector3 nearest_point = point_a;
				Vector3 farthest_point = point_b;
				if(Vector3.Distance(hit.point, point_a) > Vector3.Distance(hit.point, point_b)){
					nearest_point = point_b;
					farthest_point = point_a;
				}
				Vector3 hit_normal = hit.normal.normalized;
				Vector3 to_point = farthest_point - nearest_point;
				to_point = to_point.normalized;
				Debug.Log(hit_normal);
				Debug.Log(to_point);

				float min_dis = Vector3.Distance(nearest_point, hit.point);

				if(!(Vector3.Angle(to_point,hit_normal) <= 90.0f && min_dis < raycast_ignore_distance)){//if we are colliding with the end wall/surface
					return true;
				}
			}
		}
		return false;
	}

	void handle_lightning_damage_list(RaycastHit[] hit_list)
	{
		foreach (RaycastHit hit in hit_list) {
			GameObject obj = hit.collider.gameObject;
			if(obj.tag == player_tag){
				handle_player_hit(obj, hit.point);
			}
			else if(obj.tag == enemy_tag){
				handle_enemy_hit(obj, hit.point);
			}
		}
	}

	void handle_invalid_lightning()
	{
		end_lightning_effect();
		Debug.Log("Lightning endpoints are invalid" + Time.time.ToString());
	}

	void handle_player_hit(GameObject obj, Vector3 point)
	{
		obj.GetComponent<PlayerController>().player_health.take_damage(get_damage_amount());
        play_damage_sound(point);
	}

	void handle_enemy_hit(GameObject obj, Vector3 point)
	{
		obj.GetComponent<EnemyController>().enemy_health.take_damage(get_damage_amount());
        play_damage_sound(point);
    }

    void play_damage_sound(Vector3 point)
    {
        if (zapSource.isPlaying)
            return;
        zapSource.transform.position = point;
        zapSource.Play();
    }

	float get_damage_amount()
	{
		return damage_over_time * Time.deltaTime;
	}

	void handle_lightning_animation(Vector3 point_a, Vector3 point_b)
	{
		GameObject new_animation = Instantiate(lightning_animation_prefab) as GameObject;
		_active_animation = new_animation.GetComponent<LightningAnimation>();
		_active_animation.set_points(point_a, point_b);
	}

}
