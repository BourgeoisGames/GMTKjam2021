using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpText : MonoBehaviour
{
//	public float seconds_to_continue = 5f;
//	public GameObject continue_text;
	
	
	public string[] key_presses_to_clear;
	private Dictionary<string, bool> keys_pressed;
	public float auto_clear = 15f;
	
	void Start() {
		keys_pressed = new Dictionary<string, bool>();
		foreach (string key in key_presses_to_clear) {
			keys_pressed[key] = false;
		}
	}
	
	private bool should_clear() {
		bool should_clear_ = true;
		foreach (KeyValuePair<string, bool> entry in keys_pressed) {
			should_clear_ &= entry.Value;
		}
		return should_clear_;
	}
	
	private void check_keypresses() {
		List<string> _pressed = new List<string>();
		foreach (KeyValuePair<string, bool> entry in keys_pressed) {
			if (Input.GetKeyDown(entry.Key)) {
				_pressed.Add(entry.Key);
			}
		}
		foreach (string key in _pressed) {
			keys_pressed[key] = true;
		}
	}

    // Update is called once per frame
    void Update()
    {
		check_keypresses();
		
		if(should_clear()) {
			clear();
		}
		auto_clear -= Time.deltaTime;
		if (auto_clear < 0) {
			clear();
		}
    }
	
	public void clear() {
		gameObject.SetActive(false);
	}
}
