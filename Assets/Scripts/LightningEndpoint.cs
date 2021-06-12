using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEndpoint : MonoBehaviour
{
    // Apparent height of the lightning endpoint
    private float pylonHeight = 0.35f;

	public Transform endpoint_transform {
		get { return transform; }
	}

    public Vector3 get_lightning_position()
    {
        return endpoint_transform.position + endpoint_transform.up.normalized * pylonHeight;
    }

    public void despawn()
    {
        // Despawn the lightning ball, very simple for now
        Destroy(gameObject);
    }
}
