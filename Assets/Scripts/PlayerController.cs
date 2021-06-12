using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Camera camera;
	public float move_speed = 1f;
	public float camera_height = 1.8f;
	public Rigidbody rigidbody;
	
	public float max_camera_angle = 90f;
	
	public float mouse_sensitivity = 1f;
	private float mouse_sensitivity_x = 1f;
	private float mouse_sensitivity_y = 1f;
	
	private bool invert_y_axis = true;
	
    // Start is called before the first frame update
    void Start()
    {
		update_camera_position();
//		camera.transform.rotation = transform.rotation;
    }
	

    // Update is called once per frame
    void Update() 
	{
		roated_camera();
    }
	
	
	void FixedUpdate() 
	{
		move_player();
		update_camera_position();
	}
	
	private void roated_camera() 
	{
		float x = mouse_sensitivity * mouse_sensitivity_x * Input.GetAxis("Mouse X");
		float y = mouse_sensitivity * mouse_sensitivity_y * Input.GetAxis("Mouse Y");
		if (invert_y_axis) {
			y *= -1;
		}
		Vector3 old_angle = camera.transform.eulerAngles;
		
		float new_x = x + old_angle.y;
		float new_y = y + old_angle.x;
		
		Vector3 new_angle = new Vector3(new_y, new_x, 0);
		camera.transform.rotation = Quaternion.Euler(new_angle);
//		Vector3 new_rotation = camera.transform.eulerAngles + new Vector3(y, x, 0);
	}
	
	
	
	private void update_camera_position() 
	{
		camera.transform.position = new Vector3(0, camera_height, 0) + transform.position;
	}
	
	private void move_player() 
	{
		float move_forward = Input.GetAxis("Vertical");
		float move_right = Input.GetAxis("Horizontal");
		
		Vector3 v_forward = move_speed * move_forward * camera.transform.forward;
		Vector3 v_right = move_speed * move_right * camera.transform.right;
		
        Vector3 move = v_forward + v_right;
        //Vector3 move = new Vector3(move_forward, 0, move_right);
		
		rigidbody.velocity = move;
    }
}
