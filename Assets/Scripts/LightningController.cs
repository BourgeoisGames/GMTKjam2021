using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
	
	public GameObject prefab;

	public GameObject lightning_animation_prefab;
	private LightningAnimation _active_animation;

	public float damage_over_time = 2;
	public float max_lightning_range = 100000000.0f;

	private bool _lightning_is_active;
	
	private string player_tag = "PlayerCharacter";
	private string enemy_tag = "Enemy";
	private string wall_tag = "Wall";

	//private float raycast_ignore_distance;

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
	
	public void activate(Vector3 target) {
		if (_lighting_points.Count >= 2) {
			reset_balls();
		}
		spawn_ball(target);
		if (_lighting_points.Count == 2) {
			start_lightning_effect();
		}
	}
	
	private void start_lightning_effect() {
		_lightning_is_active = true;
		handle_lightning_animation(_lighting_points[0].endpoint_transform.position, _lighting_points[1].endpoint_transform.position);
	}

	private void end_lightning_effect() {
		_lightning_is_active = false;
		if(_active_animation != null){
			_active_animation.despawn();
		}
	}
	
	private void reset_balls() {
		foreach (LightningEndpoint ball in _lighting_points) {
			ball.despawn();
		}
		_lighting_points = new List<LightningEndpoint>();
		end_lightning_effect();
	}
	
	private void spawn_ball(Vector3 target) {
		GameObject new_ball = Instantiate(prefab) as GameObject;
		new_ball.transform.position = target;
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
		Vector3 point_a = lightning_point_a.endpoint_transform.position;
		Vector3 point_b = lightning_point_b.endpoint_transform.position;

		Vector3 direction = point_b - point_a;
		direction = direction.normalized;

		float distance = Vector3.Distance(point_a, point_b);

		if(!is_valid_lightning_distance(distance)){
			handle_invalid_lightning();
		}

		RaycastHit[] hit_list = Physics.RaycastAll(point_a, direction, distance);

		if(has_invalid_lightning_hits(hit_list, point_a, point_b)){
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

				if(!(Vector3.Angle(to_point,hit_normal) <= 90.0f)){//if we are colliding with the end wall/surface
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
				handle_player_hit(obj);
			}
			else if(obj.tag == enemy_tag){
				handle_enemy_hit(obj);
			}
		}
	}

	void handle_invalid_lightning()
	{
		end_lightning_effect();
		Debug.Log("Lightning endpoints are invalid");
	}

	void handle_player_hit(GameObject obj)
	{
		obj.GetComponent<PlayerController>().player_health.take_damage(get_damage_amount());
	}

	void handle_enemy_hit(GameObject obj)
	{
		obj.GetComponent<EnemyController>().enemy_health.take_damage(get_damage_amount());
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
