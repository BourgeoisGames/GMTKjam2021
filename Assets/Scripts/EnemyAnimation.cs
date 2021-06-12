using UnityEngine;
using System.Collections;

public class EnemyAnimation : MonoBehaviour {
	
	
	private Animator anim;
	private int battle_state = 0;
	public float speed = 6.0f;
	public float runSpeed = 3.0f;
	public float turnSpeed = 60.0f;
	public float gravity = 20.0f;
	private Vector3 moveDirection = Vector3.zero;
	private float w_sp = 0.0f;
	private float r_sp = 0.0f;
	
	// Use this for initialization
	void Start () 
	{						
		anim = GetComponent<Animator>();
		w_sp = speed; //read walk speed
		r_sp = runSpeed; //read run speed
		battle_state = 0;
		runSpeed = 1;

	}
	
	// Update is called once per frame
	void Update () 
	{		
		if(battle_state == 0){
			anim.SetInteger ("moving", 1);
		}
		else if(battle_state == 1){
			anim.SetInteger ("moving", 3);
		}

	}

	public void set_attacking(bool value){
		if(value){
			//anim.SetInteger ("battle", 1);
			battle_state = 1;
		}
		else{
			//anim.SetInteger ("battle", 0);
			battle_state = 0;
		}
	}
		
}



