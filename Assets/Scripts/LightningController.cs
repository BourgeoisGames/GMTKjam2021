using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
	
	public GameObject prefab;
	
	private List<LightningEndpoint> _lighting_points;
	
	void Start() {
		_lighting_points = new List<LightningEndpoint>();
	}
	
	public void activate(Vector3 target) {
		Debug.Log("Some call me Tim!");
		if (_lighting_points.Count >= 2) {
			reset_balls();
		}
		spawn_ball(target);
		if (_lighting_points.Count == 2) {
			start_lightning_effect();
		}
	}
	
	private void start_lightning_effect() {
		// TODO --- do something...
	}
	
	private void reset_balls() {
		foreach (LightningEndpoint ball in _lighting_points) {
			ball.despawn();
		}
		_lighting_points = new List<LightningEndpoint>();
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
	
}
