using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

	public GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 get_direction()
    {
    	Vector3 direction = target.transform.position - transform.position;
        return direction;
    }

}
