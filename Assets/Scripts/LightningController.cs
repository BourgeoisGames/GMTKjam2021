using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
	
	public GameObject prefab;
	
	private List<GameObject> _lighting_points;
	
	void Start() {
		_lighting_points = new List<GameObject>();
	}
	
	public void activate(Vector3 target) {
		Debug.Log("Some call me Tim!");
		GameObject ball = Instantiate(prefab) as GameObject;
		ball.transform.position = target;
	}
	
}
