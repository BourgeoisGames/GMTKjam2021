using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Camera camera;
	public float move_speed = 1f;
	public float camera_height = 1.8f;
	public Rigidbody rigidbody;
	
	public float mouse_sensitivity = 1f;
	private float mouse_sensitivity_x = 1f;
	private float mouse_sensitivity_y = 1f;
	
	private bool invert_y_axis = true;
	
    // Start is called before the first frame update
    void Start()
    {
		set_camera_position();
//		camera.transform.rotation = transform.rotation;
    }
	

    // Update is called once per frame
    void Update() 
	{
    }
	
	
	void FixedUpdate() 
	{
		move_player();
		set_camera_position();
		roated_camera();
	}
	
	private void roated_camera() 
	{
		float x = mouse_sensitivity * mouse_sensitivity_x * Input.GetAxis("Mouse X");
		float y = mouse_sensitivity * mouse_sensitivity_y * Input.GetAxis("Mouse Y");
		
		camera.transform.rotation  = Quaternion.Euler(new Vector3(-y, x, 0) + camera.transform.eulerAngles);
//		Vector3 new_rotation = camera.transform.eulerAngles + new Vector3(y, x, 0);
	}
	
	private void set_camera_position() 
	{
		camera.transform.position = new Vector3(0, camera_height, 0) + transform.position;
	}
	
	private void move_player() 
	{
		float move_forward = Input.GetAxis("Vertical");
		float move_right = Input.GetAxis("Horizontal");
		
		Vector3 v_forward = move_forward * camera.transform.forward;
		Vector3 v_right = move_right * camera.transform.right;
		
        Vector3 move = v_forward + v_right;
        //Vector3 move = new Vector3(move_forward, 0, move_right);
		
		rigidbody.velocity = move;
    }
}
