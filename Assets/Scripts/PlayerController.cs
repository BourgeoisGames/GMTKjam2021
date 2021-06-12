using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public LightningController lightning;
	public Camera camera;
	public PlayerHealth player_health;
    public PlayerScore player_score;
	public float move_speed = 1f;
	public float camera_height = 1.8f;
	public Rigidbody rigidbody;
	public string primary_fire = "mouse 0";
	public long lightning_range = 3;

    public float score_per_second = 1.0f;
	
	public float max_camera_angle =85f;
	
	public float mouse_sensitivity = 1f;
	private float mouse_sensitivity_x = 1f;
	private float mouse_sensitivity_y = 1f;
	
	private bool invert_y_axis = true;

    private Vector3 mouseEuler;
	
	private static PlayerController _instance;
	public static PlayerController instance {
		get { return _instance; }
	}
	
	public Transform player_transform {
		get { return transform; }
	}
	
	void Awake() {
		_instance = this;
	}
	
    // Start is called before the first frame update
    void Start()
    {
        mouseEuler = Vector3.zero;
        update_camera_position();
        //		camera.transform.rotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked;
    }
	

    // Update is called once per frame
    void Update() 
	{
		roated_camera();
        update_camera_position();
        activate_lightning();

        player_score.add_score(score_per_second * Time.deltaTime);
    }
	
	private void activate_lightning() {
		/** check if player is inputting to activate, and activate it if needed.*/
		if (Input.GetKeyDown(primary_fire)) {
			get_point_looked_at();
		}
	}
	
	private void get_point_looked_at() {
		// TODO --- add raycast layers here; should filter out enemies, if possible.
		int layerMask = Physics.DefaultRaycastLayers;
		RaycastHit hit;
		if(Physics.Raycast(camera.transform.position, camera.transform.TransformDirection(Vector3.forward), out hit, lightning_range)) {
			lightning.activate(hit);
		}
	}
	
	void FixedUpdate() 
	{
		move_player();
	}
	
	private void roated_camera() 
	{
		float x = mouse_sensitivity * mouse_sensitivity_x * Input.GetAxis("Mouse X");
		float y = mouse_sensitivity * mouse_sensitivity_y * Input.GetAxis("Mouse Y");
		if (invert_y_axis) {
			y *= -1;
		}
		Vector3 old_angle = mouseEuler;
		
		float new_x = x + old_angle.y;
		float new_y = y + old_angle.x;
        new_y = Mathf.Clamp(new_y, -max_camera_angle, max_camera_angle);
		
		Vector3 new_angle = new Vector3(new_y, new_x, 0);
        mouseEuler = new_angle;
		camera.transform.rotation = Quaternion.Euler(new_angle);
//		Vector3 new_rotation = camera.transform.eulerAngles + new Vector3(y, x, 0);
	}
	
	
	
	private void update_camera_position() 
	{
		camera.transform.position = new Vector3(0, camera_height, 0) + rigidbody.transform.position;
	}
	
	private void move_player() 
	{
		float move_forward = Input.GetAxis("Vertical");
		float move_right = Input.GetAxis("Horizontal");
		
		Vector3 v_forward = move_speed * move_forward * Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up).normalized;
		Vector3 v_right = move_speed * move_right * Vector3.ProjectOnPlane(camera.transform.right, Vector3.up).normalized;
//		Vector3 v_right = move_speed * move_right * camera.transform.right;
		
        Vector3 move = v_forward + v_right;
        //Vector3 move = new Vector3(move_forward, 0, move_right);
		
		rigidbody.velocity = move;
    }
}
